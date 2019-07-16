using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PipeLineComponents;

namespace PipePlot
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileNameInput = "C:\\Users\\Danne\\Dropbox\\Programmering\\PipePlot\\Code\\Version1\\PipePlot\\Apa.txt";
            string fileNameOutput = "Output.scad";

            for (int i = 0; i < args.Length; i++)   
            {
                Console.WriteLine(string.Format("Processing argument {0} = \"{1}\"", i, args[i]));
                switch (args[i])
                {
                    case "-input":
                        Console.WriteLine(string.Format("-input = \"{0}\"", args[i+1]));
                        fileNameInput = args[i + 1];
                        i += 1;
                        break;

                    case "-output":
                        Console.WriteLine(string.Format("-output = \"{0}\"", args[i + 1]));
                        fileNameOutput = args[i + 1];
                        i += 1;
                        break;

                    default:
                        break;
                }
            }
            

            PipeSystem System1 = new PipeSystem();

            Console.WriteLine("STEP 1: READING FROM FILE");
            System1.ReadFile(fileNameInput);

            Console.WriteLine("STEP 2: FINDING CONNECTIONS");
            System1.ConnectSystem();

            Console.WriteLine("STEP 3: ASSIGNING COORDS");
            System1.AssignCoordinates(new CoordsXYZ(0.00, 5.00, 100.0), 1, System1.Components[0]);

            Console.WriteLine("STEP 5: CHECKING");
            System1.WriteToFile(fileNameOutput);

            Console.WriteLine("STEP 6: CHECKING");
            //System1.Debug();

            //var log = new MyLogger();
            //log.LogError("Hej {0} {1}", "apa");


            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }

    class PipeSystem
    {
        public List<IPipeLineComponent> Components = new List<IPipeLineComponent>();

        private MyLogger logger = new MyLogger();

        public PipeSystem()
        {
            logger.Level = 3;
        }

        public void ReadFile(string Filename)
        {
            List<IPipeLineComponent> ComponentsReadRaw = new List<IPipeLineComponent>();
            string previousType = "";

            logger.Debug("Reading file {0}", Filename);

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(Filename))
                {
                    // Read the stream to a string, and write the string to the console.
                    string line = String.Empty;
                    logger.Write("   - Reading: ");
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] inputs = line.Split(';');

                        // If previous component read was a pipe segment and current one is not - add a pipe separator
                        if (inputs[0] != "Pipe" && previousType == "Pipe")
                        {
                            ComponentPipeSegment endOfPipe = new ComponentPipeSegment("");
                            endOfPipe.Type = "Pipe";
                            endOfPipe.UniqueId = "ENDOFPIPE";    // Todo: Dirty fix
                            ComponentsReadRaw.Add(endOfPipe);
                            logger.Write("{0}, ", endOfPipe.UniqueId);
                        }
                        previousType = inputs[0];

                        switch (inputs[0])
                        {
                            case "Pipe":
                                ComponentPipeSegment newPipeSegment = new ComponentPipeSegment(line);
                                ComponentsReadRaw.Add(newPipeSegment);
                                logger.Write("{0}, ", newPipeSegment.UniqueId);
                                break;

                            case "Valve":
                            case "Connection":
                                ComponentValve newValve = new ComponentValve(line);
                                ComponentsReadRaw.Add(newValve);
                                logger.Write("{0}, ", newValve.UniqueId);
                                break;

                            case "Tank":
                                ComponentTank newTank = new ComponentTank(line);
                                ComponentsReadRaw.Add(newTank);
                                logger.Write("{0}, ", newTank.UniqueId);
                                break;

                            default:
                                break;
                        }

                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            logger.WriteLine("");

            logger.Debug("Components in 'ComponentsReadRaw':");
            foreach (IPipeLineComponent component in ComponentsReadRaw)
            {
                logger.Debug("   " + component.BaseProperties.TypeNameId);
            }


            // Loop through 'ComponentsReadRaw' and merge pipe segment components inte pipe components
            // that contains pipe segments of the same size. Store everything in 'Components'
            IPipeLineComponent previousComponent = new ComponentTank("");
            ComponentPipe currentPipe = new ComponentPipe("");
            foreach (IPipeLineComponent currentComponent in ComponentsReadRaw)
            {
                logger.Debug("Loopar {0}",currentComponent.BaseProperties.TypeNameId);
                switch (currentComponent.BaseProperties.Type)
                {
                    case "Pipe":
                        // Cast the currentComponent to a pipe segment object
                        ComponentPipeSegment currentPipeSegment = (ComponentPipeSegment)currentComponent;

                        if (previousComponent.BaseProperties.Type != "Pipe")  // If first pipe segment
                        {
                            logger.Debug("   - Första pipesegment hittat");
                            currentPipe = new ComponentPipe(currentPipeSegment);
                            currentPipe.AddSegment(currentPipeSegment);
                        }
                        else  // If not first segment
                        {
                            logger.Debug("   - inte första pipesegment");
                            ComponentPipeSegment previousPipeSegment = (ComponentPipeSegment)previousComponent;
                            if (currentPipeSegment.DiameterOuter == previousPipeSegment.DiameterOuter && currentPipeSegment.WallThickness == previousPipeSegment.WallThickness) // Same pipe
                            {
                                logger.Debug("   - pipesegment har samma dimensioner som föregående");
                                currentPipe.AddSegment(currentPipeSegment);
                            }
                            else if (currentPipeSegment.UniqueId == "ENDOFPIPE")
                            {
                                logger.Debug("   - pipeseparator funnen");
                                Components.Add(currentPipe);
                                currentPipe = new ComponentPipe("");
                            }
                            else  // Change of dimension - add a connector of some sort
                            {
                                logger.Debug("   - pipe ändrar dimension här");
                                Components.Add(currentPipe);
                                ComponentValve newConnector = new ComponentValve("");
                                newConnector.Type = "Connection";
                                newConnector.Name = "Reducer";
                                newConnector.Connections.Add( new BaseConnection(previousPipeSegment.UniqueId, 2) );
                                newConnector.Connections.Add( new BaseConnection(currentPipeSegment.UniqueId, 1));
                                Components.Add(newConnector);
                                currentPipe = new ComponentPipe(currentPipeSegment);
                                currentPipe.AddSegment(currentPipeSegment);
                            }
                        }
                        
                        break;

                    default:
                        Components.Add(currentComponent);
                        break;
                }
                previousComponent = currentComponent;
            }

        }

        public void ConnectSystem()
        {
            string uniqueIdToLookFor;
            bool connectionFound;
            foreach (IJunctionComponent junction in Components.OfType<IJunctionComponent>())
            {
                for (int i = 0; i < junction.Connections.Count; i++)
                {
                    uniqueIdToLookFor = junction.Connections[i].Id;
                    if (junction.Connections[i].Component == null)
                    {
                        connectionFound = false;
                        foreach (IPipeLineComponent component in Components)
                        {
                            if (component.HasId(uniqueIdToLookFor) == true)
                            {
                                junction.Connections[i].Component = component;
                                connectionFound = true;
                                break;
                            }
                        }
                        if (connectionFound == false) { logger.Warning("Connection {0} of '{1}' not found", i,  junction.BaseProperties.TypeNameId); }
                    }
                }
            }
        }

        public void Debug()
        {
            Console.WriteLine(Environment.NewLine + "Components read:");
            foreach (IPipeLineComponent component in Components)
            {
                Console.WriteLine(string.Format("'{0}': Read OK", component.BaseProperties.TypeNameId));
            }

            Console.WriteLine(Environment.NewLine + "Connecting components:");
            foreach (IJunctionComponent junction in Components.OfType<IJunctionComponent>())
            {
                try
                {
                    Console.WriteLine(string.Format("'{0}': Connects from '{1}' and to '{2}'", junction.BaseProperties.TypeNameId, junction.Connections[0].Component.BaseProperties.TypeNameId, junction.Connections[1].Component.BaseProperties.TypeNameId));
                }
                catch { }
            }

            Console.WriteLine(Environment.NewLine + "Component output:");
            foreach (IPipeLineComponent component in Components)
            {
                Console.WriteLine(component.WriteOutput());
            }
        }

        public void AssignCoordinates(CoordsXYZ CoordsToSet, int Node, IPipeLineComponent CurrentComponent, string SegmentId = "")
        {
            logger.Debug("- Assigning coordinates for '{0}'", CurrentComponent.BaseProperties.TypeNameId);

            // If component coord is already set, quit
            if (CurrentComponent.CoordsIsSet == true)
            {
                logger.Debug("   - Coordinates already assigned");
                return;
            }
            else
            {
                CurrentComponent.SetCoordinates(new CoordsXYZ(CoordsToSet.X, CoordsToSet.Y, CoordsToSet.Z), Node, SegmentId);
                logger.Debug("   - Coordinates of {0} set to Coords1={1} and Coords2={2}", CurrentComponent.BaseProperties.TypeNameId, CurrentComponent.BaseProperties.Coords1.Repr(), CurrentComponent.BaseProperties.Coords2.Repr());
            }

            

            // Loop through its connections to see what it connects to - assign Coordinates to these
            if (CurrentComponent is IJunctionComponent)
            {
                logger.Debug("   - Looping through its {0} connections", CurrentComponent.BaseProperties.Connections.Count);
                
                for (int i = 0; i < CurrentComponent.BaseProperties.Connections.Count; i++)
                {
                    BaseConnection connection = CurrentComponent.BaseProperties.Connections[i];

                    CoordsXYZ CoordsToSetToConnectingComponent = i == 0 ? CurrentComponent.BaseProperties.Coords1 : CurrentComponent.BaseProperties.Coords2;
                    logger.Debug("      - Connection {0} of '{1}': {2} is assigned coords={3}", i, CurrentComponent.BaseProperties.TypeNameId, connection.Repr(), CoordsToSetToConnectingComponent.Repr());
                    AssignCoordinates(CoordsToSetToConnectingComponent, connection.Node, CurrentComponent.BaseProperties.Connections[i].Component, CurrentComponent.BaseProperties.Connections[i].Id);
                }
            }
            else { logger.Debug("   - No connections in component"); }

            // Loop through every junction/connecting component to see if it connects to current component, if it does - run assignCoordinates

            foreach (IPipeLineComponent connectingComponent in Components)
            {
                if ( !(connectingComponent is IJunctionComponent)) { continue; }
                for (int i = 0; i < connectingComponent.BaseProperties.Connections.Count; i++)
                {
                    BaseConnection connection = connectingComponent.BaseProperties.Connections[i];
                    if (connection.Component == CurrentComponent)
                    {
                        logger.Debug("   - Junction {0}/{1}/{2} connects to {3}/{4}/{5}", connectingComponent.BaseProperties.Type, connectingComponent.BaseProperties.Name, connectingComponent.BaseProperties.UniqueId, CurrentComponent.BaseProperties.Type, CurrentComponent.BaseProperties.Name, CurrentComponent.BaseProperties.UniqueId);
                        CoordsXYZ CoordsToSetToConnectingJunction = CurrentComponent.GetCoordinates(connection.Node, connection.Id);
                        AssignCoordinates(CoordsToSetToConnectingJunction, i + 1, connectingComponent, connectingComponent.BaseProperties.UniqueId);
                    }
                }
            }
        }

        public void WriteToFile(string Filename)
        {
            logger.Debug("Writing to file '{0}'", Filename);

            FileStream f = new FileStream(Filename, FileMode.Create);
            StreamWriter s = new StreamWriter(f);


            s.WriteLine("$fn=50;");
            s.WriteLine("use <curvedPipe.scad>");
            s.WriteLine("");
            s.WriteLine("colorPipe = \"grey\";");
            s.WriteLine("colorReducer = \"grey\";");
            s.WriteLine("colorValve = \"red\";");
            s.WriteLine("colorTank = \"grey\";");
            s.WriteLine("");


            s.WriteLine("scale([1000, 1000, 1000])");
            s.WriteLine("{{");

            foreach (IPipeLineComponent component in Components)
            {
                s.WriteLine(component.WriteOutput());

            }
            s.WriteLine("}}");
            s.Close();
            f.Close();
            Console.WriteLine("File created successfully...");
        }
    }

    public class MyLogger
    {
        public int Level { get; set; }

        /*
        5. DEBUG
        4. INFO
        3. WARNING
        2. ERROR
        1. CRITICAL
        */

        public void Error(string format, params object[] args)
        {
            if (Level >= 2) { Console.WriteLine(string.Format(format, args)); }
        }

        public void Warning(string format, params object[] args)
        {
            if (Level >= 3) { Console.WriteLine(string.Format(format, args)); }
        }

        public void Debug(string format, params object[] args)
        {
            if (Level >= 5) { Console.WriteLine(string.Format(format, args)); }
        }

        public void Write(string format, params object[] args)
        {
            Console.Write(string.Format(format, args));
        }
        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }
    }
}





namespace PipeLineComponents
{
    public interface IPipeLineComponent
    {
        bool CoordsIsSet { get; set; }

        BaseComponent BaseProperties { get; }

        string WriteOutput();
        bool HasId(string UniqueId);

        void SetCoordinates(CoordsXYZ NewCoords, int Node, string SegmentId);
        CoordsXYZ GetCoordinates(int Node, string SegmentId);

    }

    public interface IJunctionComponent
    {
        BaseComponent BaseProperties { get; }

        List<BaseConnection> Connections { get; set; }
    }

    public class BaseConnection
    {
        public string Id { get; set; }
        public int Node { get; set; }
        public IPipeLineComponent Component { get; set; }

        public BaseConnection(string Id, int Node)
        {
            this.Id = Id;
            this.Node = Node;
        }
        public BaseConnection(string Id, string NodeStr)
        {
            this.Id = Id;
            int i = -1;
            if (!Int32.TryParse(NodeStr, out i))
            {
                i = -1;

            }
            this.Node = i;
        }

        public string Repr() { return string.Format("[Id={0}/Node={1}]", Id, Node); }
    }

    public class CoordsXYZ
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public bool IsSet {get; set; }
        
        public CoordsXYZ() { }
        public CoordsXYZ(double X0, double Y0, double Z0)
        {
            IsSet = true;
            this.X = X0; this.Y = Y0; this.Z = Z0;
        }

        public string Repr() { return string.Format("[{0},{1},{2}]", X, Y, Z); }
    }

    public class BaseComponent
    {
        public string Name { get; set; }
        public string UniqueId { get; set; }
        public string Type { get; set; }

        public string TypeNameId { get { return string.Format("Type={0}/Name={1}/Id={2}", Type, Name, UniqueId); } }

        public BaseComponent BaseProperties{ get { return this; } }

        public List<BaseConnection> Connections { get; set; }

        public double Length { get; set; }
        public double AngleVertical { get; set; }
        public double AngleAzimuthal { get; set; }
        public double Dx { get; set; }
        public double Dy { get; set; }
        public double Dz { get; set; }
        public double DiameterOuter { get; set; }
        public double WallThickness { get; set; }
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }
        public bool CoordsIsSet { get; set; }
        public CoordsXYZ Coords1 { get; set; }
        public CoordsXYZ Coords2 { get; set; }

        public BaseComponent(string InputString)
        {
            string[] inputWords = InputString.Split(';');
            this.Connections = new List<BaseConnection>();
            this.Coords1 = new CoordsXYZ();
            this.Coords2 = new CoordsXYZ();

            if (inputWords.Length >= 17)
            {
                this.Type = inputWords[0];
                this.UniqueId = inputWords[1];
                this.Length = Convert.ToDouble(inputWords[2]);
                this.AngleVertical = Convert.ToDouble(inputWords[3]);
                this.AngleAzimuthal = Convert.ToDouble(inputWords[4]);
                this.Dx = Convert.ToDouble(inputWords[5]);
                this.Dy = Convert.ToDouble(inputWords[6]);
                this.Dz = Convert.ToDouble(inputWords[7]);
                this.DiameterOuter = Convert.ToDouble(inputWords[8]);
                this.WallThickness = Convert.ToDouble(inputWords[9]);
                this.Name = inputWords[10];

                BaseConnection connectionFrom = new BaseConnection(inputWords[11], inputWords[12]);
                this.Connections.Add(connectionFrom);

                BaseConnection connectionTo = new BaseConnection(inputWords[13], inputWords[14]);
                this.Connections.Add(connectionTo);

                this.Parameter1 = inputWords[15];
                this.Parameter2 = inputWords[16];
            }

        }

        public void SetCoordinates(CoordsXYZ NewCoords, int Node, string SegmentId = "")
        {
            double deg2rad = Math.PI / 180;
            switch (Node)
            {
                case 1:
                    Coords1.X = NewCoords.X; Coords1.Y = NewCoords.Y; Coords1.Z = NewCoords.Z;
                    Coords2.X = Coords1.X + Length * Math.Cos(AngleVertical * deg2rad) * Math.Cos(AngleAzimuthal * deg2rad) + Dx;
                    Coords2.Y = Coords1.Y + Length * Math.Cos(AngleVertical * deg2rad) * Math.Sin(AngleAzimuthal * deg2rad) + Dy;
                    Coords2.Z = Coords1.Z + Length * Math.Sin(AngleVertical * deg2rad) + Dz;
                    break;
                case 2:
                    Coords2.X = NewCoords.X; Coords2.Y = NewCoords.Y; Coords2.Z = NewCoords.Z;
                    Coords1.X = Coords2.X - Length * Math.Cos(AngleVertical * deg2rad) * Math.Cos(AngleAzimuthal * deg2rad) - Dx;
                    Coords1.Y = Coords2.Y - Length * Math.Cos(AngleVertical * deg2rad) * Math.Sin(AngleAzimuthal * deg2rad) - Dy;
                    Coords1.Z = Coords2.Z - Length * Math.Sin(AngleVertical * deg2rad) - Dz;
                    break;
                default:
                    break;
            }
            CoordsIsSet = true;
        }

        public CoordsXYZ GetCoordinates(int Node, string SegmentId)
        {
            if (Node == 1) { return Coords1; }else { return Coords2; }
        }
    }

    public class ComponentValve : BaseComponent, IPipeLineComponent, IJunctionComponent
    {
        public ComponentValve(string InputString) : base(InputString) { }

        public string WriteOutput()
        {
            var valveString = new StringBuilder();
            valveString.AppendFormat("    // Valve {1}{0}", Environment.NewLine, this.Name);
            valveString.AppendFormat("    color(colorValve){0}", Environment.NewLine);
            valveString.AppendFormat("    translate({1}){0}", Environment.NewLine, this.Coords1.Repr() );
            valveString.AppendFormat("    rotate([0,{1},{2}]){0}", Environment.NewLine, -this.AngleVertical, this.AngleAzimuthal);
            valveString.AppendFormat("    rotate([0, 90, 0]){0}", Environment.NewLine);
            valveString.AppendFormat("    translate([0, 0, {1}]){0}", Environment.NewLine, this.Length/2);
            valveString.AppendFormat("    {{ {0}", Environment.NewLine);
            valveString.AppendFormat("        cylinder(h = {1}, r = {2}, center = true);{0}", Environment.NewLine, this.Length, this.DiameterOuter/2000);
            valveString.AppendFormat("    }} {0}", Environment.NewLine);
            return valveString.ToString();
        }

        public bool HasId(string UniqueIdToLookFor)
        {
            return UniqueId == UniqueIdToLookFor ? true : false;
        }
    }

    public class ComponentTank : BaseComponent , IPipeLineComponent
    {
        public ComponentTank(string InputString) : base(InputString) { }

        public string WriteOutput()
        {
            var tankString = new StringBuilder();
            tankString.AppendFormat("    // Valve {1}{0}", Environment.NewLine, this.Name);
            tankString.AppendFormat("    color(colorTank){0}", Environment.NewLine);
            tankString.AppendFormat("    translate({1}){0}", Environment.NewLine, this.Coords1.Repr());
            tankString.AppendFormat("    rotate([0,{1},{2}]){0}", Environment.NewLine, -this.AngleVertical, this.AngleAzimuthal);
            tankString.AppendFormat("    rotate([0, 90, 0]){0}", Environment.NewLine);
            tankString.AppendFormat("    translate([0, 0, {1}]){0}", Environment.NewLine, this.Length / 2);
            tankString.AppendFormat("    {{ {0}", Environment.NewLine);
            tankString.AppendFormat("        cylinder(h = {1}, r = {2}, center = true);{0}", Environment.NewLine, this.Length, this.DiameterOuter / 2000);
            tankString.AppendFormat("    }} {0}", Environment.NewLine);
            return tankString.ToString();
        }

        public bool HasId(string UniqueIdToLookFor)
        {
            return UniqueId == UniqueIdToLookFor ? true : false;
        }
    }

    public class ComponentPipe : BaseComponent , IPipeLineComponent
    {
        public List<ComponentPipeSegment> Segments = new List<ComponentPipeSegment>();

        public new string Name
        {
            get
            {
                string Result = "";
                foreach (ComponentPipeSegment segment in Segments)
                {
                    Result += segment.UniqueId + ",";
                }
                return Result; }
            set {    }
        }

        public ComponentPipe(string InputString) : base(InputString) { }

        public ComponentPipe(ComponentPipeSegment SegmentToUseAsBase) : base("")
        {
            Type = "PipeRun";
            Name = SegmentToUseAsBase.Name;
            UniqueId = SegmentToUseAsBase.UniqueId;

        }

        public void AddSegment(ComponentPipeSegment NewPipeSegment)
        {
            Segments.Add(NewPipeSegment);
        }

        public string WriteOutput()
        {
            string NodeString = "[";
            string BendRadiusString = "[";
            double Do = Segments[0].DiameterOuter * 0.001;
            double Di = Do - 2*Segments[0].WallThickness*0.001;

            for (int i = 0; i < Segments.Count; i++)
            {
                NodeString += Segments[i].Coords1.Repr();
                NodeString += i == Segments.Count - 1 ? "," + Segments[i].Coords2.Repr() : "";
                NodeString += i == Segments.Count - 1 ? "]" : ",";

                BendRadiusString += Convert.ToDouble(Segments[i].Parameter1)*Do;
                BendRadiusString += i == Segments.Count - 1 ? "]" : ",";
            }

            var pipeString = new StringBuilder();
            pipeString.AppendFormat("    // Pipe {1} (Id {2} to {3}){0}", Environment.NewLine, this.Name, this.Segments[0].UniqueId, this.Segments[Segments.Count-1].UniqueId);
            pipeString.AppendFormat("    color(colorPipe){0}", Environment.NewLine);
            pipeString.AppendFormat("    curvedPipe({1},{2},{3},{4},{5});{0}", Environment.NewLine, NodeString, Segments.Count, BendRadiusString, Do, Di);

            return pipeString.ToString();

            //Console.WriteLine(string.Format("This is a multi segment pipe component with {0} segments",Segments.Count));
        }
        public bool HasId(string UniqueIdToLookFor)
        {
            bool Result = false;
            foreach (ComponentPipeSegment Segment in Segments)
            {
                if (Segment.UniqueId == UniqueIdToLookFor)
                {
                    Result = true;
                    break;
                }
            }
            return Result;
        }

        private int GetSegmentIndex(string UniqueIdToLookFor)
        {
            int Result = -1;
            for (int i = 0; i < Segments.Count; i++)
            {
                if (Segments[i].UniqueId == UniqueIdToLookFor)
                {
                    Result = i;
                    break;
                }
            }
            return Result;
        }

        public new void SetCoordinates(CoordsXYZ NewCoords, int Node, string SegmentId = "")
        {
            int segmentIndex = GetSegmentIndex(SegmentId);

            if (segmentIndex == -1) {
                Console.WriteLine(string.Format("SegmentId {0} not found in {1}", SegmentId, Name));
                return;
            }

            // Assign coordinates to segment with ID 'SegmentId'
            Segments[segmentIndex].SetCoordinates(new CoordsXYZ(NewCoords.X, NewCoords.Y, NewCoords.Z), Node);
            
            // Assign coordinates from segment after segment with ID 'SegmentId' to last segment
            for (int i = segmentIndex+1; i < Segments.Count; i++)
            {
                Segments[i].SetCoordinates(Segments[i - 1].Coords2, 1);
            }

            // Assign coordinates from segment before segment with ID 'SegmentId' to first segment
            for (int i = segmentIndex - 1; i >= 0; i--)
            {
                Segments[i].SetCoordinates(Segments[i + 1].Coords1, 2);
            }

            // Assign global end coordinates X1, X2, ..., Z2
            Coords1 = Segments[0].Coords1;
            Coords2 = Segments[Segments.Count - 1].Coords2;

            CoordsIsSet = true;
        }

        public new CoordsXYZ GetCoordinates(int Node, string SegmentId)
        {
            CoordsXYZ Result = new CoordsXYZ();
            foreach (ComponentPipeSegment segment in Segments)
            {
                if (segment.UniqueId == SegmentId)
                {
                    if (Node == 1) { return segment.Coords1; } else { return segment.Coords2; }
                }
            }
            return Result;
        }
    }

    public class ComponentPipeSegment : BaseComponent , IPipeLineComponent
    {
        public ComponentPipeSegment(string InputString) : base(InputString) { }

        public bool HasId(string UniqueId)
        {
            throw new NotImplementedException();
        }

        public string WriteOutput()
        {
            return "";
        }
    }

}
