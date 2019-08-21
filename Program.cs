using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PipeLineComponents;
using Bloat;

namespace PipePlot
{
    /// <summary>
    /// Main program
    /// </summary>
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
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
            

            XlsPipePlotMain xlsPipePlot = new XlsPipePlotMain(fileNameInput);            
            xlsPipePlot.WriteToFile(fileNameOutput);

            if (Logger.NumberOfWarnings > 0 || Logger.Level == 5)
            {
                Console.WriteLine("Press any key to quit");
                Console.Read();
            }
            
        }
    }

    /// <summary>
    /// Contains all methods for 1) Reading the file, 2) Connecting the system(s) and 3) Assigning coordinates
    /// </summary>
    public class XlsPipePlotMain
    {
        /// <summary>The main components library located in a subfolder to the executable.</summary>
        private string PathMain = AppDomain.CurrentDomain.BaseDirectory + "Components\\";

        /// <summary>The local components library located in a subfolder to the input file.</summary>
        private string PathLocal = "C:\\Users\\Daniel\\Dropbox\\Programmering\\PipePlot\\Code\\Version1\\PipePlot\\LocComponents\\";

        /// <summary>A list containing all components in the order they were read.</summary>
        public List<BaseComponent> Components = new List<BaseComponent>();

        /// <summary>A list containing the reference volumes of each system.</summary>
        public List<ReferenceVolume> RefVols = new List<ReferenceVolume>();

        /// <summary>A dictionary containing the flowpath captions. The key represent before what component index they are placed.</summary>
        public IDictionary<int, string> Flowpaths = new Dictionary<int, string>();

        /// <summary>
        /// Main constructor for XlsPipePlotMain class. It is instanciated with a input file that is processed in the following way
        /// 1. Components and settings are read and stored.
        /// 2. The system are connected by looping through all the BaseConnections of each components.
        /// 3. The coordinates of each component and their segments are calculated.
        /// 4. How the components and their segments are to be printed (OpenSCAD input format) are stored in a BaseTemplate object.
        ///    In this step each segment is first assigned a default template file depending on the type (Pipe, Valve, Reducer, etc).
        ///    If the name field (word 12) of the segment have the file extension .scad information in this file located either in
        ///    the local or main components directory are read into the segment object. If no file is present a template file is created in
        ///    the local components directory.
        /// </summary>
        /// <param name="Filename"></param>
        public XlsPipePlotMain(string Filename)
        {
            PathLocal = Path.GetDirectoryName(Filename) + "\\Components\\";
            ReadFile(Filename);
            
            System.IO.Directory.CreateDirectory(PathLocal);  // Create Pathlocal (TODO: Don't create the folder if no scad file are to be created)

            ConnectSystem();

            AssignCoordinates();
            
            AssignFiles();
        }

        /// <summary>
        /// Reads the semi colon separated input file
        /// </summary>
        /// <param name="Filename">The file to read</param>
        public void ReadFile(string Filename)
        {
            Console.WriteLine("");
            Console.WriteLine("*** STEP 1: READING FROM FILE ***");
            Logger.Debug("Reading file {0}", Filename);

            var previousSegment = new BaseSegment("");
            var currentSegment = new BaseSegment("");
            
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(Filename))
                {
                    // Read the stream to a string, and write the string to the console.
                    string line = String.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] inputs = line.Split(';');

                        switch (inputs[0])
                        {
                            case "Pipe":
                            case "Volume":
                            case "Component":
                                currentSegment = new BaseSegment(line);

                                if (currentSegment.Type != previousSegment.Type)
                                {
                                    var newComponent = new BaseComponent(currentSegment);
                                    Components.Add(newComponent);
                                }
                                else if(currentSegment.Type == previousSegment.Type)
                                {
                                    // If Pipe and changing dimensions - add a reducer between
                                    // TODO: Also include i a discrete coord change (dx, dy or dz) is given
                                    if (currentSegment.Type == "Pipe" && (currentSegment.DiameterOuter != previousSegment.DiameterOuter || currentSegment.WallThickness != previousSegment.WallThickness) )
                                    {
                                        var newReducer = new BaseComponent(line);
                                        newReducer.Segments[0].Type = "Reducer";
                                        newReducer.Segments[0].UniqueId = "-";
                                        newReducer.Segments[0].Length = 0.00;
                                        newReducer.Segments[0].DiameterOuter = previousSegment.DiameterOuter;
                                        newReducer.Segments[0].Parameter1 = currentSegment.DiameterOuter;
                                        if (previousSegment.DiameterOuter <= currentSegment.DiameterOuter)
                                            newReducer.Segments[0].Filename = "ReducerStraightExp.scad";
                                        else
                                            newReducer.Segments[0].Filename = "ReducerStraightRed.scad";
                                        
                                        newReducer.Segments[0].Connections[0].TargetSegment = previousSegment;
                                        newReducer.Segments[0].Connections[1].TargetSegment = currentSegment;
                                        newReducer.Segments[0].Connections[0].TargetId = previousSegment.UniqueId;
                                        newReducer.Segments[0].Connections[1].TargetId = currentSegment.UniqueId;
                                        newReducer.Segments[0].Connections[0].TargetNode = 2;
                                        newReducer.Segments[0].Connections[1].TargetNode = 1;
                                        Components.Add(newReducer);

                                        Components.Add(new BaseComponent(currentSegment));
                                    }
                                    else  // If other component
                                    {
                                        Components[Components.Count - 1].AddSegment(currentSegment);
                                    }
                                }
                                break;

                            case "Refvol":
                                RefVols.Add(new ReferenceVolume(line));
                                break;

                            case "Flowpath":
                                Flowpaths.Add(Components.Count, inputs[1]);
                                break;

                            default:
                                currentSegment = new BaseSegment("");
                                break;
                        }
                        previousSegment = currentSegment;
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Loops through all the connections in all segments in all components and looks for its connection. When 
        /// the unique id is found this segment is stored in the BaseConnection object representing the connection
        /// </summary>
        public void ConnectSystem()
        {
            Console.WriteLine("");
            Console.WriteLine("*** STEP 2: FINDING CONNECTIONS ***");

            string uniqueIdToLookFor;
            bool connectionFound;

            // Loop through each component and its connections and make connections
            foreach (BaseComponent component in Components)
            {
                foreach (BaseConnection connection in component.Connections())
                {
                    // If current connection is not made (only ID as string present and not segment object), look for it in the
                    //  other components
                    if (connection.IsConnected == false)  
                    {
                        uniqueIdToLookFor = connection.TargetId;
                        connectionFound = false;
                        var numberOfSegmentsFound = 0;
                        Logger.Debug("Looking for Id='{0}'", uniqueIdToLookFor);

                        foreach (BaseComponent componentToLookIn in Components)
                        {
                            foreach (BaseSegment segment in componentToLookIn.Segments)
                            {
                                if (segment.UniqueId == uniqueIdToLookFor)
                                {
                                    numberOfSegmentsFound += 1;
                                    connectionFound = true;
                                    Logger.Debug("    Found connection");
                                    if (numberOfSegmentsFound == 1)
                                        connection.TargetSegment = segment;
                                }
                            }
                        }

                        if (connectionFound == false)
                            Logger.Warning("Connection {2} in segment with Id={0} to segment with Id={1} not found", connection.SourceSegment.UniqueId, connection.TargetId, connection.SourceNode); 

                        if (numberOfSegmentsFound > 1)
                            Logger.Warning("Connection {2} in segment with Id={0} refers to a segment id ({1}) where multiple occurances was found", connection.SourceSegment.UniqueId, connection.TargetId, connection.SourceNode);
                    }
                }
            }
            foreach (BaseComponent component in Components)
                foreach (BaseConnection connection in component.Connections())
                    Logger.Debug("Connection from node {0} of {1} connects to {3} at a distance {4} m from node {2}", connection.SourceNode, connection.SourceSegment.UniqueId, connection.TargetNode, connection.TargetSegment.UniqueId, connection.AxialTranslation);
        }



        /// <summary>
        /// Loop through each component and assign suitable .scad-files to each segment.
        /// </summary>
        public void AssignFiles()
        {
            Console.WriteLine("");
            Console.WriteLine("*** STEP 4: ASSIGNING DEFAULT AND USER SPECIFIED TEMPLATE FILES ***");

            IDictionary<string, string> defaultTemplateFiles = new Dictionary<string, string>()
                                            { {"Tank","TankDefault.scad"}, {"Valve", "ValveDefault.scad"}, {"Reducer","ReducerStraight.scad"}, {"Connection", "Cylinder.scad"} };
            IDictionary<string, string> defaultColors = new Dictionary<string, string>()
                                            { {"Pipe","grey"}, {"Tank","grey"}, {"Valve", "red"}, {"Reducer","red"}, {"Connection", "red"} };

            foreach (BaseComponent component in Components)
            {
                // Instanciate both component and segment template without file
                component.Template = new BaseTemplate(component);
                foreach (BaseSegment segment in component.Segments)
                    segment.Template = new BaseTemplate(component, segment);

                // Set default colors
                if (component.Color == null)
                {
                    string defaultColor;
                    if (!defaultColors.TryGetValue(component.Type, out defaultColor))
                        defaultColor = "green";
                    component.Color = defaultColor;
                }

                // If pipe, no custom .scad files are allowed
                if (component.Type == "Pipe")
                {
                    component.Template.ReadTemplateFile(PathMain + "PipeSegment.scad");
                    continue;
                }

                // Loop through each segment and depending on its type and other properties assign it a suitable .scad file
                List<string> localFilenames = new List<string>();
                foreach (BaseSegment segment in component.Segments)
                {
                    // Assign default template files for each segment
                    string fn;
                    if (!defaultTemplateFiles.TryGetValue(component.Type, out fn))
                        fn = "Cylinder.scad";

                    // Update template with informaton from default template files
                    segment.Template.ReadTemplateFile(PathMain + fn);

                    // Check if file exists first in local folder, then in main folder
                    if (segment.Filename != "")
                    {
                        // If template file exist in local folder AND main folder - use file in local folder
                        // If template file exist only in main folder - use this file
                        // If doesn't exist anywhere - create file in local folder (at a later stage)
                        if (File.Exists(PathLocal + segment.Filename) == true)
                        {
                            segment.Filename = PathLocal + segment.Filename;
                            localFilenames.Add(segment.Filename);
                        }
                        else if (File.Exists(PathMain + segment.Filename) == true)
                        {
                            segment.Template.ReadTemplateFile(PathMain + segment.Filename);
                            segment.Filename = "";
                        }
                        else
                        {
                            segment.Filename = PathLocal + segment.Filename;
                            localFilenames.Add(segment.Filename);
                        }
                    }
                }

                localFilenames = localFilenames.Distinct().ToList();

                // Update component template with information from from specified template files
                if (localFilenames.Count == 1 && component.Segments.Count > 1)
                {
                    component.Filename = localFilenames[0];

                    // Update component template with information read from local file (if file doesn't exists it is created)
                    component.Template.ReadTemplateFile(component.Filename);
                }

                // Update segment template with information from from specified template files (both in local and main path)
                foreach (BaseSegment segment in component.Segments)
                {
                    if (segment.Filename != "")
                        segment.Template.ReadTemplateFile(segment.Filename);
                }
            }
        }

        public void AssignCoordinates()
        {
            Console.WriteLine("");
            Console.WriteLine("*** STEP 3: ASSIGNING COORDS ***");
            foreach (ReferenceVolume referenceVolume in RefVols)
            {
                foreach (BaseComponent component in Components)
                {
                    if (component.IndexOf(referenceVolume.SegmentId) != -1)   // TODO: If component already assigned coords produce an error message
                        SetCoordinates(component, referenceVolume.SegmentId, referenceVolume.Node, 0.0, referenceVolume.Coords);
                }
            }
            if (RefVols.Count == 0)
            {
                Logger.Warning("No reference volume given. Setting Node 1 of {0} to (x,y,z)=(0,0,0)", Components[0].Segments[0].UniqueId);
                SetCoordinates(Components[0], Components[0].Segments[0].UniqueId, 1, 0.0, new CoordsXYZ(0, 0, 0));
            }

            foreach (BaseComponent component in Components)
            {
                if (component.CoordsIsSet == false)
                {
                    Logger.Warning("No coordinates calculated for {0}. Setting node 1 of its first segment ({1}) as a reference volume with (x,y,z)=(0,0,0)", component.TypeNameId, component.Segments[0].UniqueId);
                    SetCoordinates(component, component.Segments[0].UniqueId, 1, 0.0, new CoordsXYZ(0, 0, 0));
                }
            }

            // Loop check
            foreach (BaseComponent component in Components)
            {
                foreach (BaseConnection connection in component.Connections(OnlyCompleteConnections: true))
                {
                    if (connection.CoordsWithinTolerance() == false)
                    {
                        Logger.Warning("Loop check for connection {0} of segment '{1}' failed. [dx,dy,dx] = {2} m", connection.SourceNode, connection.SourceSegment.UniqueId, connection.GetCoordsMismatch().Repr());
                    }
    
                }
            }

        }

        /// <summary>
        /// Recursive method to 
        /// 1) Assign coordinates to component (exit if already done), 
        /// 2) Loop through its connections and run this method on these and...
        /// 3) Loop through every connection in all components and see if they attach to this component
        /// </summary>
        /// <param name="CoordsToSet">The coordinates to assign to the component</param>
        /// <param name="Node">The node (1 or 2, inlet or outlet) where the coordinates apply</param>
        /// <param name="CurrentComponent">The component object</param>
        /// <param name="SegmentId">The segment id</param>
        public void SetCoordinates(BaseComponent CurrentComponent, string SegmentId, int Node, double AxialTranslation, CoordsXYZ CoordsToSet)
        {
            Logger.Debug("- Assigning coordinates for '{0}'", CurrentComponent.TypeNameId);

            // If component coord is already set, quit
            if (CurrentComponent.CoordsIsSet == true)
            {
                Logger.Debug("   - Coordinates already assigned");
                return;
            }
            else
            {
                CurrentComponent.SetCoordinates(SegmentId, Node, AxialTranslation, new CoordsXYZ(CoordsToSet.X, CoordsToSet.Y, CoordsToSet.Z));
                Logger.Debug("   - Coordinates of {0} set to Coords1={1} and Coords2={2}", CurrentComponent.Name, CurrentComponent.Coords1.Repr(), CurrentComponent.Coords2.Repr());
            }
                       
            // Loop through its connections to see what it connects to - assign Coordinates to these
            foreach (BaseConnection connection in CurrentComponent.Connections(OnlyCompleteConnections: true))
            {
                //var CoordsToSetToConnectingComponent = connection.SourceNode == 1 ? connection.SourceSegment.Coords1 : connection.SourceSegment.Coords2;
                var CoordsToSetToConnectingComponent = connection.SourceSegment.GetCoordinates(connection.SourceNode);

                Logger.Debug("      - Connection {0} of '{1}': {2} is assigned coords={3}", -1, CurrentComponent.Name, connection.Repr(), CoordsToSetToConnectingComponent.Repr());
                SetCoordinates(connection.TargetSegment.Parent, connection.TargetId, connection.TargetNode, connection.AxialTranslation, CoordsToSetToConnectingComponent);
            }

            // Loop through every junction/connecting component to see if it connects to current component, if it does - run assignCoordinates
            foreach (BaseComponent component in Components)
            {
                foreach (BaseConnection connection in component.Connections(OnlyCompleteConnections: true))
                {
                    if (CurrentComponent.IndexOf(connection.TargetSegment) != -1)
                    {
                        //var CoordsToSetToConnectingJunction = connection.TargetNode == 1 ? connection.TargetSegment.Coords1 : connection.TargetSegment.Coords2;   // CurrentComponent.GetCoordinates(connection.TargetNode, connection.TargetId);
                        var CoordsToSetToConnectingJunction = connection.TargetSegment.GetCoordinates(connection.TargetNode, connection.AxialTranslation);
                        SetCoordinates(component, connection.SourceSegment.UniqueId, connection.SourceNode, 0.0, CoordsToSetToConnectingJunction);
                    }
                }
            }
         }

        /// <summary>
        /// Writes to file
        /// </summary>
        /// <param name="Filename">The OpenSCAD output file</param>
        public void WriteToFile(string Filename)
        {
            Console.WriteLine("");
            Console.WriteLine("*** STEP 5: WRITING TO FILE ***");

            Logger.Debug("Writing to file '{0}'", Filename);

            FileStream f = new FileStream(Filename, FileMode.Create);
            StreamWriter s = new StreamWriter(f);

            s.WriteLine("// Generated " + DateTime.Now.ToString());
            s.WriteLine("$fn=50;");
            s.WriteLine("use <curvedPipe.scad>");
            s.WriteLine("");
            s.WriteLine("colorPipe = \"grey\";");
            s.WriteLine("colorReducer = \"grey\";");
            s.WriteLine("colorValve = \"red\";");
            s.WriteLine("colorTank = \"grey\";");
            s.WriteLine("");
            s.WriteLine("");
            s.WriteLine("// Default flowpath");
            s.WriteLine("scale([1000, 1000, 1000])");
            s.WriteLine("{");

            

            foreach (BaseComponent component in Components)
            {
                // Write out a new flowpath the current index is present in the 'Flowpaths' dictionary.
                string Caption;
                if (Flowpaths.TryGetValue(Components.IndexOf(component), out Caption))
                {
                    s.WriteLine("}");
                    s.WriteLine("");
                    s.WriteLine("");
                    s.WriteLine(string.Format("// {0}", Caption));
                    s.WriteLine("scale([1000, 1000, 1000])");
                    s.WriteLine("{");
                }
                    
                s.WriteLine(component.Template.OutputComponent(IndentLevel: 1));
            }
            s.WriteLine("}");
            s.WriteLine("");
            s.WriteLine("");

            // Write out fauly connections (those where the source and the target coords are larger than tolerance) as 
            // lines between source and target node (if any)
            s.WriteLine("// Faulty connections");
            s.WriteLine("module line(start, end, thickness = 0.010)");
            s.WriteLine("{");
            s.WriteLine("    $fn=10;");
            s.WriteLine("    hull()");
            s.WriteLine("    {");
            s.WriteLine("        translate(start) sphere(thickness);");
            s.WriteLine("        translate(end) sphere(thickness);");
            s.WriteLine("    }");
            s.WriteLine("}");
            s.WriteLine("");

            s.WriteLine("scale([1000, 1000, 1000])");
            s.WriteLine("{");

            foreach (BaseComponent component in Components)
            {
                foreach (BaseConnection connection in component.Connections(OnlyCompleteConnections: true))
                {
                    if (connection.CoordsWithinTolerance() == false)
                        s.WriteLine(string.Format("    line({0}, {1});  // Connection {2} of {3}", connection.SourceSegment.GetCoordinates(connection.SourceNode).Repr(), connection.TargetSegment.GetCoordinates(connection.TargetNode).Repr(), connection.SourceNode, connection.SourceSegment.UniqueId));
                }
            }

            s.WriteLine("}");
            s.Close();
            f.Close();
            Console.WriteLine("File created successfully...");
        }       
    }
}





namespace PipeLineComponents
{
    /// <summary>
    /// Class that for a basic pipeline component containing one or more segments and connections
    /// </summary>
    public class BaseComponent
    {
        /// <summary>The name of the component. Equals the name of the first segment if multiple segments in component</summary>
        public string Name { get { return Segments[0].Name; } }

        /// <summary>The type of the component</summary>
        public string Type { get { return Segments[0].Type; } }

        /// <summary>The color of the component</summary>
        public string Color { get; set; }

        /// <summary>A string representaton of the component</summary>
        public string TypeNameId { get { return string.Format("Type={0}/Name={1}/Id={2}", Type, Name, Segments[0].UniqueId); } }

        /// <summary>
        /// If only one .scad-file is given for each segment in the component, this is the filename for the component.
        /// It is used to generate a aggregate .scad-file consisting of many segments
        /// </summary>
        /// <value>Empty string if no filename is given or component consists of many .scad-files</value>
        public string Filename { get; set; }

        /// <summary>Object that stores the component template information (OpenSCAD code written with keywords instead of actual values)</summary>
        public BaseTemplate Template { get; set; }

        /// <summary>A list containing the segments in the component</summary>
        public List<BaseSegment> Segments { get; set; }

        /// <summary>True</summary>
        /// <value>True if coordinates is set, false otherwise</value>
        public bool CoordsIsSet { get; set; }

        /// <summary>The CoordsXYZ object of the inlet (node 1 of segment 0) of the component</summary>
        public CoordsXYZ Coords1 { get { return Segments[0].Coords1; } }

        /// <summary>The CoordsXYZ object of the outlet (node 2 of segment N) of the component</summary>
        public CoordsXYZ Coords2 { get { return Segments[Segments.Count - 1].Coords2; } }

        /// <summary>Default constructor for creating a BaseComponent object from an comma separated input string.</summary>
        /// <param name="InputString">A semi colon separated string with segment input for the first segment of the component</param>
        public BaseComponent(string InputString)
        {
            Segments = new List<BaseSegment>();
            var firstSegment = new BaseSegment(InputString);
            Segments.Add(firstSegment);
        }

        /// <summary>Default constructor for creating a BaseComponent object from an existing BaseSegment object.</summary>
        /// <param name="FirstSegment">A BaseSegment object for the first segment of the component</param>
        public BaseComponent(BaseSegment FirstSegment)
        {
            Segments = new List<BaseSegment>();
            Segments.Add(FirstSegment);
            FirstSegment.Parent = this;
        }

        /// <summary>Adds a segment object to the segment list</summary>
        /// <param name="NewSegment">A segment object for the segment to add to the component</param>
        public void AddSegment(BaseSegment NewSegment)
        {
            NewSegment.Parent = this;
            Segments.Add(NewSegment);
        }

        /// <summary>Sets the coordinates (xyz) to all the segments of the component given coordinates and a reference location (segment and node)</summary>
        /// <param name="NewCoords">The coords of the reference location</param>
        /// <param name="Node">The node (1 or 2) of the reference segment</param>
        /// <param name="SegmentId">The unique id of the reference segment</param>
        public void SetCoordinates(string SegmentId, int Node, double AxialTranslation, CoordsXYZ NewCoords)
        {
            int segmentIndex = IndexOf(SegmentId);

            if (segmentIndex == -1)
            {
                Console.WriteLine(string.Format("SegmentId {0} not found in {1}", SegmentId, Name));
                return;
            }

            // Assign coordinates to segment with ID 'SegmentId'
            Segments[segmentIndex].SetCoordinates(new CoordsXYZ(NewCoords.X, NewCoords.Y, NewCoords.Z), Node, AxialTranslation);

            // Assign coordinates from segment after segment with ID 'SegmentId' to last segment
            for (int i = segmentIndex + 1; i < Segments.Count; i++)
            {
                Segments[i].SetCoordinates(Segments[i - 1].Coords2, 1);
            }

            // Assign coordinates from segment before segment with ID 'SegmentId' to first segment
            for (int i = segmentIndex - 1; i >= 0; i--)
            {
                Segments[i].SetCoordinates(Segments[i + 1].Coords1, 2);
            }

            CoordsIsSet = true;
        }

        /// <summary>
        /// Method that returns the index of a certain segment
        /// </summary>
        /// <param name="UniqueIdToLookFor">A string with the segment id</param>
        /// <returns>An integer representing the segment index or -1 if not found in component</returns>
        public int IndexOf(string UniqueIdToLookFor)
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

        /// <summary>
        /// Method that returns the index of a certain segment
        /// </summary>
        /// <param name="SegmentToLookFor">The segment object to look for</param>
        /// <returns>An integer representing the segment index or -1 if not found in component</returns>
        public int IndexOf(BaseSegment SegmentToLookFor)
        {
            int Result = -1;
            for (int i = 0; i < Segments.Count; i++)
            {
                if (Segments[i] == SegmentToLookFor)
                {
                    Result = i;
                    break;
                }
            }
            return Result;
        }

        /// <summary>
        /// Iterator method to access all connections in the component
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseConnection> Connections(bool OnlyCompleteConnections = false)
        {
            foreach (BaseSegment segment in Segments)
            {
                foreach (BaseConnection connection in segment.Connections)
                {
                    if (OnlyCompleteConnections == false)
                    {
                        if (connection.IsValid == true)
                            yield return connection; 
                    }
                    else
                    {
                        if (connection.IsConnected == true)
                            yield return connection;
                    }
                        
                }
            }
        }
    }

    /// <summary>
    /// Class that reads and stores the segment properties (one line of input). A segment is a subset of a component that contains one or more segments.
    /// </summary>
    public class BaseSegment
    {
        private string _name;

        /// <summary>The name of the segment</summary>
        public string Name
        {
            get { return _name; }
            set
            {
                // If name contains a .scad extension, add to filename
                if (value.Right(5) == ".scad")
                {
                    Filename = value;
                    _name = value.Substring(0, value.Length - 5);
                }
                else
                    _name = value;
            }
        }

        /// <summary>The unique id of the segment. Used to establish connections.</summary>
        public string UniqueId { get; set; }

        /// <summary>The type of the segment. Pipe, valve etc.</summary>
        public string Type { get; set; }

        /// <summary>The drawing number. For information purpose only.</summary>
        public string Drawing { get; set; }

        /// <summary>Notes. For information purpose only.</summary>
        public string Notes { get; set; }

        /// <summary>
        /// The filename of the .scad file to be used as a main template. Located in the a subfolder to the input file (usually named 'Components')
        /// The file contains OpenSCAD input for a segment oriented in the positive x-axis.
        /// </summary>
        /// <value>A absolute path to the .scad file. Null if not set.</value>
        public string Filename { get; set; }

        /// <summary>
        /// Object that stores the segment template information (OpenSCAD code written with keywords instead of actual values)
        /// </summary>
        public BaseTemplate Template { get; set; }

        /// <summary>The length of the segment in meters</summary>
        public double Length { get; set; }

        /// <summary>The vertical angle of the segment. Between -90 and +90 degrees.</summary>
        public double AngleVertical { get; set; }

        /// <summary>The azimuthal angle of the segment. Between 0 and 360 degrees clockwise around z-axis. 0 degrees is towards the +x-axis, 90 degrees is towards -y-axis and so on.</summary>
        public double AngleAzimuthal { get; set; }

        /// <summary>The angle around the segment axis/center. Used for instance to align the valve bonnet. Between 0 and 360 degrees.</summary>
        public double AngleAxis { get; set; }

        /// <summary>Discrete x-coordinate change over the segment (in addition to that given by the length and the angles)</summary>
        public double Dx { get; set; }

        /// <summary>Discrete y-coordinate change over the segment (in addition to that given by the length and the angles)</summary>
        public double Dy { get; set; }

        /// <summary>Discrete z-coordinate change over the segment (in addition to that given by the length and the angles)</summary>
        public double Dz { get; set; }

        /// <summary>The outer diameter in meters</summary>
        public double DiameterOuter { get; set; }

        /// <summary>The wall thickness in meters</summary>
        public double WallThickness { get; set; }

        /// <summary>Parameter 1 for the segment. Used differently depending on type of component. For Pipe this is the bend radius r/Do.</summary>
        /// <value>
        /// For Pipe: r/Do&#xA;
        /// For Valve: Apparent length
        /// </value>
        public double Parameter1 { get; set; }

        /// <summary>Parameter 2 for the segment.</summary>
        public double Parameter2 { get; set; }

        /// <summary>The absolute coordinates (xyz) of node 1 (inlet) of the segment</summary>
        public CoordsXYZ Coords1 { get; set; }

        /// <summary>The absolute coordinates (xyz) of node 2 (outlet) of the segment</summary>
        public CoordsXYZ Coords2 { get; set; }

        /// <summary>A list with the connections of the segment</summary>
        public List<BaseConnection> Connections { get; set; }

        /// <summary>The BaseComponent object current segment is a child of</summary>
        public BaseComponent Parent { get; set; }

        /// <summary>
        /// Default constructor for creating a BaseSegment object from a semicolon separated input line.
        /// W1 = Type, W2 = Unique-Id, W3 = Length, W4 = Vertical angle, W5 = Azimuthal angle, W6 = Angle around segment axis, 
        /// W7 = Dx, W8 = Dy, W9 = Dz, W10 = Outer diameter, W11 = Wall thickness, W12 = Name, W13 = From segment, W14 = From node, 
        /// W15 = To segment, W16 = To node, W17 = Parameter 1, W18 = Parameter 2
        /// </summary>
        /// <param name="InputString"></param>
        public BaseSegment(string InputString)
        {
            string[] inputWords = InputString.Split(';');
            Coords1 = new CoordsXYZ();
            Coords2 = new CoordsXYZ();
            Connections = new List<BaseConnection>();

            if (inputWords.Length >= 22)
            {
                // Loop through all input values that supposed to be doubles and try converting them. Produce an error message if incorrect.
                double tmpDbl;
                List<int> notNumericList = new List<int>();
                foreach (int index in new int[] {5, 6,7,8,9,10,11,12,13})
                {
                    if (!Double.TryParse(inputWords[index], out tmpDbl))
                        notNumericList.Add(index + 1);
                }
                if (notNumericList.Count > 0) Logger.Warning("{0}Input error: Word(s) {1} in {2} not numeric", Environment.NewLine, string.Join(", ", notNumericList), inputWords[1]);

                Type = inputWords[1];
                Name = inputWords[2];
                UniqueId = inputWords[3];
                Filename = inputWords[4];
                Drawing = inputWords[20];
                Notes = inputWords[21];
                
                if (!Double.TryParse(inputWords[5], out tmpDbl)) tmpDbl = 0.0;
                Length = tmpDbl;
                if (!Double.TryParse(inputWords[6], out tmpDbl)) tmpDbl = 0.0;
                AngleVertical = tmpDbl;
                if (!Double.TryParse(inputWords[7], out tmpDbl)) tmpDbl = 0.0;
                AngleAzimuthal = tmpDbl;
                if (!Double.TryParse(inputWords[8], out tmpDbl)) tmpDbl = 0.0;
                AngleAxis = tmpDbl;
                if (!Double.TryParse(inputWords[9], out tmpDbl)) tmpDbl = 0.0;
                Dx = tmpDbl;
                if (!Double.TryParse(inputWords[10], out tmpDbl)) tmpDbl = 0.0;
                Dy = tmpDbl;
                if (!Double.TryParse(inputWords[11], out tmpDbl)) tmpDbl = 0.0;
                Dz = tmpDbl;
                if (!Double.TryParse(inputWords[12], out tmpDbl)) tmpDbl = 0.0;
                DiameterOuter = tmpDbl * 0.001;
                if (!Double.TryParse(inputWords[13], out tmpDbl)) tmpDbl = 0.0;
                WallThickness = tmpDbl * 0.001;
                if (!Double.TryParse(inputWords[14], out tmpDbl)) tmpDbl = 0.0;
                Parameter1 = tmpDbl;
                if (!Double.TryParse(inputWords[15], out tmpDbl)) tmpDbl = 0.0;
                Parameter2 = tmpDbl;

                // Read connections
                for (int i = 0; i < 2; i++)
                {
                    string targetId = (inputWords[16 + i*2] == "-") ? "" : inputWords[16 + i*2];
                    int targetNode;
                    double axialTranslation = 0.0;
                    if (inputWords[17 + i*2] == "A")
                    {
                        targetNode = 1;
                    }
                    else if (inputWords[17 + i*2] == "B")
                    {
                        targetNode = 2;
                    }
                    else
                    {
                        targetNode = 1;
                        if (!Double.TryParse(inputWords[17 + i*2], out axialTranslation))
                            axialTranslation = 0;

                    }
                    Connections.Add(new BaseConnection(this, i + 1, targetId, targetNode, axialTranslation));
                }
                
                
            }
            else
            {
                //Logger.Warning("Not enough words: '{0}'", InputString);
            }
        }

        /// <summary>
        /// Sets the segment coordinates for inlet and outlet (node 1 and 2 or A and B)
        /// </summary>
        /// <param name="NewCoords">The new coordinates to set to segment</param>
        /// <param name="Node">The location (node 1 or 2, inlet or outlet) where the coordinates apply</param>
        public void SetCoordinates(CoordsXYZ NewCoords, int Node, double AxialTranslation = 0.0)
        {
            double deg2rad = Math.PI / 180;
            switch (Node)
            {
                case 1:
                    Coords1.X = NewCoords.X - AxialTranslation * Math.Cos(AngleVertical * deg2rad) * Math.Cos(AngleAzimuthal * deg2rad);
                    Coords1.Y = NewCoords.Y + AxialTranslation * Math.Cos(AngleVertical * deg2rad) * Math.Sin(AngleAzimuthal * deg2rad);
                    Coords1.Z = NewCoords.Z - AxialTranslation * Math.Sin(AngleVertical * deg2rad);

                    Coords2.X = Coords1.X + Length * Math.Cos(AngleVertical * deg2rad) * Math.Cos(AngleAzimuthal * deg2rad) + Dx;
                    Coords2.Y = Coords1.Y - Length * Math.Cos(AngleVertical * deg2rad) * Math.Sin(AngleAzimuthal * deg2rad) + Dy;
                    Coords2.Z = Coords1.Z + Length * Math.Sin(AngleVertical * deg2rad) + Dz;
                    break;
                case 2:
                    Coords2.X = NewCoords.X + AxialTranslation * Math.Cos(AngleVertical * deg2rad) * Math.Cos(AngleAzimuthal * deg2rad);
                    Coords2.Y = NewCoords.Y - AxialTranslation * Math.Cos(AngleVertical * deg2rad) * Math.Sin(AngleAzimuthal * deg2rad);
                    Coords2.Z = NewCoords.Z + AxialTranslation * Math.Sin(AngleVertical * deg2rad);

                    Coords1.X = Coords2.X - Length * Math.Cos(AngleVertical * deg2rad) * Math.Cos(AngleAzimuthal * deg2rad) - Dx;
                    Coords1.Y = Coords2.Y + Length * Math.Cos(AngleVertical * deg2rad) * Math.Sin(AngleAzimuthal * deg2rad) - Dy;
                    Coords1.Z = Coords2.Z - Length * Math.Sin(AngleVertical * deg2rad) - Dz;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Returns the CoordsXYZ of the segment (Coords1 or Coords2 depending on the node)
        /// </summary>
        /// <param name="Node">The node</param>
        /// <param name="AxialTranslation">The node</param>
        /// <returns></returns>
        public CoordsXYZ GetCoordinates(int Node, double AxialTranslation = 0.0)
        {
            double deg2rad = Math.PI / 180;
            //return (Node == 1) ? Coords1 : Coords2;
            //CoordsXYZ CoordStart, CoordEnd;
            var coordStart = (Node == 1) ? Coords1 : Coords2;
            var coordOutput = new CoordsXYZ();
            coordOutput.X = coordStart.X + AxialTranslation * Math.Cos(AngleVertical * deg2rad) * Math.Cos(AngleAzimuthal * deg2rad);
            coordOutput.Y = coordStart.Y - AxialTranslation * Math.Cos(AngleVertical * deg2rad) * Math.Sin(AngleAzimuthal * deg2rad);
            coordOutput.Z = coordStart.Z + AxialTranslation * Math.Sin(AngleVertical * deg2rad);
            return coordOutput;
        }
    }

    /// <summary>
    /// A class containing connection information (source segment and node and target segment and node). A connection is a subset of a segment.
    /// </summary>
    public class BaseConnection
    {
        /// <summary>A string with the unique id of the target of the connection</summary>
        public string TargetId { get; set; }

        /// <summary>The node on the target where the connection attaches (1 or 2, inlet or outlet)</summary>
        public int TargetNode { get; set; }

        /// <summary>The node on the source where the connection attaches (1 or 2, inlet or outlet)</summary>
        public int SourceNode { get; set; }

        /// <summary>The BaseSegment object of the target</summary>
        public BaseSegment TargetSegment { get; set; }

        /// <summary>The BaseSegment object of the source</summary>
        public BaseSegment SourceSegment { get; set; }

        /// <summary>Axial distance from TargetNode where connection is placed on TargetSegment.</summary>
        public double AxialTranslation { get; set; }

        /// <value>Returns true if TargetId is someting other than a empty string or "-"</value>
        public bool IsValid
        {
            get
            {
                if (TargetId == "-" || TargetId == "")
                    return false;
                else
                    return true;
            }
        }

        /// <value>Returns true if TargetSegment is set (connection found).</value>
        public bool IsConnected
        {
            get
            {
                if (TargetSegment == null)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// constructor for a connection object
        /// </summary>
        /// <param name="SourceSegment">The segment object of the source</param>
        /// <param name="SourceNode">The node where the connection attaches on the source segment</param>
        /// <param name="TargetId">The unique id of the target segment</param>
        /// <param name="TargetNode">The node where the connection attaches on the target segment</param>
        public BaseConnection(BaseSegment SourceSegment, int SourceNode, string TargetId, int TargetNode, double AxialTranslation = 0.0)
        {
            this.SourceSegment = SourceSegment;
            this.SourceNode = SourceNode;
            this.TargetId = TargetId;
            this.AxialTranslation = AxialTranslation;
            if (TargetId != "" && TargetNode == -1)
            {
                Logger.Warning("Incorrect node in Connection {0} '{1}', setting target node to 1", SourceNode, SourceSegment.UniqueId);
                this.TargetNode = 1;
            }
            else
            {
                this.TargetNode = TargetNode;
            }
        }

        /// <summary>
        /// Methods that checks if coords of source and target segment nodes are within tolerance
        /// </summary>
        /// <param name="Tolerance">The specified tolerance for x, y and z-direction.</param>
        /// <returns></returns>
        public bool CoordsWithinTolerance(double Tolerance = 0.01)
        {
            var difference = GetCoordsMismatch();
            if (Math.Abs(difference.X) > Tolerance || Math.Abs(difference.Y) > Tolerance || Math.Abs(difference.Z) > Tolerance)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Returns a CoordsXYZ object corresponding to the difference in coordinates x, y and z between the source segment of
        /// the connection and the target segment. Ideally they should be equal but if the loop check fails there can be a difference.
        /// A positive value of for instance Z means that the source segment is a above the segment it connects to.
        /// </summary>
        /// <returns>A CoordsXYZ object.</returns>
        public CoordsXYZ GetCoordsMismatch()
        {
            return SourceSegment.GetCoordinates(SourceNode) - TargetSegment.GetCoordinates(TargetNode, AxialTranslation);
        }

        /// <summary>
        /// Returns a string representation of the connection object for debug purpose
        /// </summary>
        /// <returns>A string</returns>
        public string Repr() { return string.Format("[SourceName={0},SourceId={1},SourceNode={2},TargetId={3},TargetNode={4}]", SourceSegment.Name, SourceSegment.UniqueId, SourceNode, TargetSegment.UniqueId, TargetNode); }
    }

    /// <summary>
    /// Class containing the template information and methods. That is: information on how to use the information stored in the component
    /// and segments to generate output.
    /// Both component objects and segment objects have a template object and if or not it has a BaseSegment object or not
    /// determines how the output is generated
    /// </summary>
    public class BaseTemplate
    {
        private string _readText = "";

        private readonly BaseComponent Component;
        private readonly BaseSegment Segment;

        public IDictionary<string, string> KeywordsAndValues = new Dictionary<string, string>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Component">The parent component of the template</param>
        /// <param name="Segment">The parent segment of the template. If it is a component template Segment = null</param>
        /// <param name="Filename">The filename (full path) of the file to be read and used as a template.</param>
        public BaseTemplate(BaseComponent Component, BaseSegment Segment = null, string Filename = "")
        {
            this.Component = Component;
            this.Segment = Segment;
            if (Filename != "")
                ReadTemplateFile(Filename);

            if (Segment != null)
            {
                KeywordsAndValues.Add("LENGTH", this.Segment.Length.ToString());
                KeywordsAndValues.Add("DO", this.Segment.DiameterOuter.ToString());
                KeywordsAndValues.Add("WALLTHICKNESS", this.Segment.WallThickness.ToString());
                KeywordsAndValues.Add("PARAM1", this.Segment.Parameter1.ToString());
                KeywordsAndValues.Add("ANGLE_VERTICAL", this.Segment.AngleVertical.ToString());
                KeywordsAndValues.Add("ANGLE_AZIMUTHAL", this.Segment.AngleAzimuthal.ToString());
                KeywordsAndValues.Add("ANGLE_AXIS", this.Segment.AngleAxis.ToString());
                KeywordsAndValues.Add("COORDS1_LOC", this.Segment.Coords1.Repr(Component.Coords1));
            }
        }

        /// <summary>
        /// Reads a file that is to be used as a template. Lines with "// REMOVE"-string will not be read.
        /// </summary>
        /// <param name="Filename">The filename (full path) of the file to be read and used as a template.</param>
        public void ReadTemplateFile(string Filename)
        {
            if (File.Exists(Filename) == false)
                WriteTemplateFile(Filename);

            var readSegment = new StringBuilder();

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(Filename))
                {
                    string line = String.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.IndexOf("// REMOVE") == -1)
                            readSegment.AppendLine(line);
                    }
                }

            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                readSegment.AppendFormat("// Error, {1} could not be read.{0}", Environment.NewLine, Filename);
            }
            _readText = readSegment.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }

        /// <summary>
        /// Returns a string with a complete component OpenSCAD input
        /// </summary>
        /// <param name="IndentLevel">The desired indentation level (0, 1, 2, etc) of the component output. Default is 0.</param>
        /// <returns>A string</returns>
        public string OutputComponent(int IndentLevel = 0)
        {
            var output = new StringBuilder();

            // If no segment is given - return output for whole component
            output.AppendFormat("// Type={1}, Name={2}, Id={3}{0}", Environment.NewLine, Component.Type, Component.Name, Component.Segments[0].UniqueId);
            output.AppendFormat("color(\"{1}\"){0}", Environment.NewLine, Component.Color);
            output.AppendFormat("translate({1}){0}", Environment.NewLine, Component.Coords1.Repr());
            output.AppendFormat("{{ {0}", Environment.NewLine);

            if (_readText != "") // If component consists of one multi-segment template file 
            {
                var componentRead = _readText;
                componentRead = componentRead.Insert(0, new string(' ', 4));
                componentRead = componentRead.Replace(Environment.NewLine, Environment.NewLine + new String(' ', 4));  // Add 1 level of indent
                output.AppendLine(componentRead);
            }
            else  // If component consists of one or several single-segment template files
            {
                foreach (BaseSegment segment in Component.Segments)
                    output.AppendLine(segment.Template.OutputSegment(1));
            }
            output.AppendFormat("}} {0}", Environment.NewLine);

            // Add requested indent before every line
            output.Insert(0, new string(' ', 4 * IndentLevel));
            output.Replace(Environment.NewLine, Environment.NewLine + new String(' ', 4 * IndentLevel));

            return ReplaceKeywords(output.ToString());
        }

        /// <summary>
        /// Returns a string with a segment OpenSCAD input constructed from a template.
        /// </summary>
        /// <param name="IndentLevel">The desired indentation level (0, 1, 2, etc) of the component output. Default is 0.</param>
        /// <param name="OutputKeywords">(Optional) If false then keywords are replaced with their corresponding values. Default is false.</param>
        /// <returns>A string</returns>
        public string OutputSegment(int IndentLevel = 0, bool OutputKeywords = false)
        {
            var output = new StringBuilder();

            output.AppendFormat("translate((COORDS1_LOC)){0}", Environment.NewLine);
            output.AppendFormat("rotate([(ANGLE_AXIS),-1*(ANGLE_VERTICAL),-1*(ANGLE_AZIMUTHAL)]){0}", Environment.NewLine);
            output.AppendFormat("{{ {0}", Environment.NewLine);

            var segmentRead = _readText;
            segmentRead = segmentRead.Insert(0, new string(' ', 4));
            segmentRead = segmentRead.Replace(Environment.NewLine, Environment.NewLine + new String(' ', 4));  // Add 1 level of indent

            output.AppendLine(segmentRead);
            output.AppendFormat("}} {0}", "");  //Environment.NewLine);

            // Add requested indent before every line
            output.Insert(0, new string(' ', 4 * IndentLevel));
            output.Replace(Environment.NewLine, Environment.NewLine + new String(' ', 4 * IndentLevel));

            if (OutputKeywords == false)
                return ReplaceKeywords(output.ToString());
            else
                return output.ToString();
        }

        /// <summary>
        /// Writes template to file
        /// </summary>
        /// <param name="Filename">The full path of the file to be created.</param>
        public void WriteTemplateFile(string Filename)
        {
            FileStream f = new FileStream(Filename, FileMode.Create);
            StreamWriter s = new StreamWriter(f);

            if (Segment == null)  // Creates a multi-segment template file
            {
                Console.WriteLine(string.Format("- Creating multi segment template file = {0}", Filename));
                s.WriteLine("$fn=50;  // REMOVE");
                int ind = 0;
                foreach (BaseSegment segment in Component.Segments)
                {
                    foreach (KeyValuePair<string, string> kvp in segment.Template.KeywordsAndValues)
                        s.WriteLine(string.Format("{0}_{1} = {2}; // REMOVE", kvp.Key, ind, kvp.Value));
                    ind += 1;
                }
                ind = 0;
                foreach (BaseSegment segment in Component.Segments)
                {
                    var segmentText = segment.Template.OutputSegment(OutputKeywords: true);
                    foreach (KeyValuePair<string, string> kvp in segment.Template.KeywordsAndValues)
                    {
                        segmentText = segmentText.Replace(string.Format("({0})", kvp.Key), string.Format("({0}_{1})", kvp.Key, ind));
                    }

                    s.WriteLine(segmentText);
                    ind += 1;
                }
            }
            else  // Creates a single-segment template file
            {
                Console.WriteLine(string.Format("- Creating single segment template file = {0}", Filename));
                s.WriteLine("$fn=50;  // REMOVE");
                foreach (KeyValuePair<string, string> kvp in Segment.Template.KeywordsAndValues)
                    s.WriteLine(string.Format("{0} = {1}; // REMOVE", kvp.Key, kvp.Value));

                s.WriteLine(_readText);
            }

            s.Close();
            f.Close();
        }

        /// <summary>
        /// Method that replaces keywords (LENGTH, DO, etc) within a string with its value
        /// </summary>
        /// <param name="str">The string containing keywords that are to be replaced with their values.</param>
        /// <returns>A string where keywords are replaced with values.</returns>
        private string ReplaceKeywords(string str)
        {
            var strOut = str;

            // If a BaseSegment is associated with this BaseTemplate object
            if (Segment != null)
            {
                foreach (KeyValuePair<string, string> kvp in Segment.Template.KeywordsAndValues)
                    strOut = strOut.Replace(string.Format("({0})", kvp.Key), kvp.Value);
            }

            // Loop through every segment and replace keywords with indexes (LENGTH_0, LENGTH_1, etc)
            // with their corresponding values.
            var ind = -1;
            foreach (BaseSegment segment in Component.Segments)
            {
                ind += 1;
                foreach (KeyValuePair<string, string> kvp in segment.Template.KeywordsAndValues)
                    strOut = strOut.Replace(string.Format("({0}_{1})", kvp.Key, ind), kvp.Value);
            }

            // If pipe component
            var nodes = "[" + string.Join(",", Component.Segments.Select(BaseSegment => BaseSegment.Coords1.Repr(Component.Coords1)).ToList()) + "," + Component.Segments[Component.Segments.Count - 1].Coords2.Repr(Component.Coords1) + "]";
            var bendradius = "[" + string.Join(",", Component.Segments.Select(BaseSegment => BaseSegment.Parameter1 * BaseSegment.DiameterOuter).ToList()) + "]";

            strOut = strOut.Replace("(DO)", Component.Segments[0].DiameterOuter.ToString());
            strOut = strOut.Replace("(WALLTHICKNESS)", Component.Segments[0].WallThickness.ToString());
            strOut = strOut.Replace("(NODES)", nodes);
            strOut = strOut.Replace("(BENDRADIUS)", bendradius);
            strOut = strOut.Replace("(SEGMENTS)", string.Format("{0}", Component.Segments.Count));

            return strOut;
        }
    }

    /// <summary>
    /// A class for the xyz coordinates
    /// </summary>
    public class CoordsXYZ
    {
        /// <summary>The x-coordinate</summary>
        public double X { get; set; }
        /// <summary>The y-coordinate</summary>
        public double Y { get; set; }
        /// <summary>The z-coordinate</summary>
        public double Z { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CoordsXYZ() { }

        /// <summary>
        /// Constructor to initiate a default
        /// </summary>
        /// <param name="X0">x-coordinate</param>
        /// <param name="Y0">y-coordinate</param>
        /// <param name="Z0">z-coordinate</param>
        public CoordsXYZ(double X0, double Y0, double Z0)
        {
            X = X0; Y = Y0; Z = Z0;
        }

        public static CoordsXYZ operator -(CoordsXYZ Object1, CoordsXYZ Object2)
        {
            return new CoordsXYZ(Object1.X - Object2.X, Object1.Y - Object2.Y, Object1.Z - Object2.Z);
        }

        /// <summary>
        /// Returns the coordinate in a vector representation [x,y,z]
        /// </summary>
        /// <param name="Origin">(Optional) A CoordXYZ object with the origin of the local coordinates</param>
        /// <returns>A String</returns>
        public string Repr(CoordsXYZ Origin = null)
        {
            if (Origin == null)
                Origin = new CoordsXYZ(0, 0, 0);
            return string.Format("[{0:F4},{1:F4},{2:F4}]", X - Origin.X, Y - Origin.Y, Z - Origin.Z);
        }
    }

    /// <summary>
    /// Class to contain informaton about reference volume locations (A node in a segment where the coords are known)
    /// </summary>
    public class ReferenceVolume
    {
        /// <summary>Returns the unique id of the segment where the coordinates are known.</summary>
        /// <value>Unique id of the segment</value>
        public string SegmentId { get; }

        /// <summary>Returns the node of the segment where the coordinates are known.</summary>
        /// <value>Unique id of the segment</value>
        public int Node { get; }

        /// <summary>The CoordsXYZ object of the coords of the reference location.</summary>
        /// <value>Coords</value>
        public CoordsXYZ Coords { get; }

        /// <summary>
        /// Default constructor for a Reference volume
        /// </summary>
        /// <param name="InputString"></param>
        public ReferenceVolume(string InputString)
        {
            string[] inputWords = InputString.Split(';');
            SegmentId = inputWords[1];

            int tmpInt;
            if (!Int32.TryParse(inputWords[2], out tmpInt)) tmpInt = -1;
            this.Node = tmpInt;

            double X, Y, Z;
            if (!Double.TryParse(inputWords[3], out X)) X = -1;
            if (!Double.TryParse(inputWords[4], out Y)) Y = -1;
            if (!Double.TryParse(inputWords[5], out Z)) Z = -1;
            this.Coords = new CoordsXYZ(X, Y, Z);
        }
    }
}




namespace Bloat
{
    /// <summary>
    /// Logger class
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// The level of what is to print out to screen.
        /// </summary>
        public static int Level = 5;

        public static int NumberOfWarnings = 0;

        /// <summary>
        /// Produces an error message
        /// </summary>
        /// <param name="format">A string.Format string</param>
        /// <param name="args">Arguments for the string.Format method</param>
        public static void Error(string format, params object[] args)
        {
            if (Level >= 2) { Console.WriteLine(string.Format(format, args)); }
        }

        /// <summary>
        /// Produces an warning message 
        /// </summary>
        /// <param name="format">A string.Format string</param>
        /// <param name="args">Arguments for the string.Format method</param>
        public static void Warning(string format, params object[] args)
        {
            NumberOfWarnings += 1;
            if (Level >= 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                //Console.Beep();
                Console.WriteLine(string.Format(format, args));
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Produces an debug message
        /// </summary>
        /// <param name="format">A string.Format string</param>
        /// <param name="args">Arguments for the string.Format method</param>
        public static void Debug(string format, params object[] args)
        {
            if (Level >= 5) { Console.WriteLine(string.Format(format, args)); }
        }

        /// <summary>
        /// Writes always visible message to console w/o line break
        /// </summary>
        /// <param name="format">A string.Format string</param>
        /// <param name="args">Arguments for the string.Format method</param>
        public static void Write(string format, params object[] args)
        {
            Console.Write(string.Format(format, args));
        }

        /// <summary>
        /// Writes always visible message to console with line break
        /// </summary>
        /// <param name="format">A string.Format string</param>
        /// <param name="args">Arguments for the string.Format method</param>
        public static void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }
    }



    /// <summary>
    /// Extends string class with left and right methods
    /// </summary>
    public static class DataTypeExtensions
    {
        #region Methods

        /// <summary>
        /// Returns the left part of the string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(this string str, int length)
        {
            str = (str ?? string.Empty);
            return str.Substring(0, Math.Min(length, str.Length));
        }

        /// <summary>
        /// Returns the right part of the string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(this string str, int length)
        {
            str = (str ?? string.Empty);
            return (str.Length >= length)
                ? str.Substring(str.Length - length, length)
                : str;
        }

        #endregion
    }
}
