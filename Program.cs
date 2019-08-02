using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PipeLineComponents;
using Bloat;

/// <summary>
/// Main program
/// </summary>
namespace PipePlot
{
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
            

            XlsPipePlotMain xlsPipePlot = new XlsPipePlotMain();

            Console.WriteLine("STEP 1: READING FROM FILE");
            xlsPipePlot.ReadFile(fileNameInput);

            Console.WriteLine("STEP 2: FINDING CONNECTIONS");
            xlsPipePlot.ConnectSystem();

            Console.WriteLine("STEP 3: ASSIGNING COORDS");
            xlsPipePlot.AssignCoordinates(new CoordsXYZ(0.00, 5.00, 100.0), 1, xlsPipePlot.Components[0], xlsPipePlot.Components[0].Segments[0].UniqueId);

            Console.WriteLine("STEP 4: ASSIGNING SEGMENT FILES FROM MAIN AND LOCAL COMPONENT LIBRARY");
            xlsPipePlot.AssignFiles();

            foreach (BaseComponent component in xlsPipePlot.Components)
            {
                Console.WriteLine(component.WriteOutput());
                Console.WriteLine();
            }

            Console.Read();
            Environment.Exit(1);

            Console.WriteLine("STEP 5: WRITING TO FILE");
            xlsPipePlot.WriteToFile(fileNameOutput);

            Console.WriteLine("Press any key to quit");
            Console.Read();
        }
    }

    /// <summary>
    /// Contains all methods for 1) Reading the file, 2) Connecting the system(s) and 3) Assigning coordinates
    /// </summary>
    class XlsPipePlotMain
    {
        public string PathMain = AppDomain.CurrentDomain.BaseDirectory + "Components\\";
        public string PathLocal = "C:\\Users\\Daniel\\Dropbox\\Programmering\\PipePlot\\Code\\Version1\\PipePlot\\LocComponents\\";

        public List<BaseComponent> Components = new List<BaseComponent>();

        private MyLogger logger = new MyLogger();

        public XlsPipePlotMain()
        {
            logger.Level = 3;
        }

        /// <summary>
        /// Reads the semi colon separated input file
        /// </summary>
        /// <param name="Filename">The file to read</param>
        public void ReadFile(string Filename)
        {
            logger.Debug("Reading file {0}", Filename);

            var previousSegment = new BaseSegment("");
            var currentSegment = new BaseSegment("");
            

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

                        
                        switch (inputs[0])
                        {
                            case "Pipe":
                            case "Tank":
                            case "Valve":
                            case "Connection":
                            case "Conn":

                                currentSegment = new BaseSegment(line);
                                logger.Write("{0}, ", currentSegment.UniqueId);

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
                                        if (previousSegment.DiameterOuter <= currentSegment.DiameterOuter)
                                        {
                                            newReducer.Segments[0].DiameterOuter = previousSegment.DiameterOuter;
                                            newReducer.Segments[0].Parameter1 = currentSegment.DiameterOuter*0.001;
                                            newReducer.Segments[0].TemplateMain = "ReducerStraightExp.scad";
                                        }
                                        else
                                        {
                                            newReducer.Segments[0].DiameterOuter = previousSegment.DiameterOuter;
                                            newReducer.Segments[0].Parameter1 = currentSegment.DiameterOuter*0.001;
                                            newReducer.Segments[0].TemplateMain = "ReducerStraightRed.scad";
                                        }
                                        
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

                                //logger.Write("{0}, ", newPipeSegment.UniqueId);
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

            logger.WriteLine("");

            logger.Debug("Components in 'Components':");
            foreach (BaseComponent component in Components)
            {
                logger.Debug("   " + component.Name);
                
            }
        }

        /// <summary>
        /// Loops through all the connections in all segments in all components and looks for its connection. When 
        /// the unique id is found this segment is stored in the BaseConnection object representing the connection
        /// </summary>
        public void ConnectSystem()
        {
            string uniqueIdToLookFor;
            bool connectionFound;

            // Loop through each component and its connections and make connections
            foreach (BaseComponent component in Components)
            {
                foreach (BaseConnection connection in component.Connections())
                {
                    // If current connection is not made (only ID as string present and not segment object), look for it in the
                    //  other components
                    if (connection.ConnectionMade == false)  
                    {
                        uniqueIdToLookFor = connection.TargetId;
                        connectionFound = false;
                        logger.Debug("   Looking for Id='{0}'", uniqueIdToLookFor);

                        foreach (BaseComponent componentToLookIn in Components)
                        {
                            int segmentIndex = componentToLookIn.IndexOf(uniqueIdToLookFor);
                            if (segmentIndex != -1)
                            {
                                logger.Debug("      Found connection");
                                connection.TargetSegment = componentToLookIn.Segments[segmentIndex];
                                connectionFound = true;
                                break;
                            }
                        }

                        if (connectionFound == false)
                            logger.Warning("      Warning: Connection not found"); 
                    }
                }
            }
        }

        /// <summary>
        /// Loop through each component and assign suitable .scad-files to each segment.
        /// </summary>
        public void AssignFiles()
        {
            IDictionary<string, string> defaultTemplateFiles = new Dictionary<string, string>()
                                            {
                                                {"Tank","TankDefault.scad"}, {"Valve", "ValveDefault.scad"},
                                                {"Reducer","ReducerStraightRed.scad"}, {"Connection", "Cylinder.scad"}
                                            };

            foreach (BaseComponent component in Components)
            {
                component.Template = new BaseTemplate(component);

                // If pipe, no custom .scad files are allowed
                if (component.Type == "Pipe")
                {
                    component.Template.ReadTemplateFile(PathMain + "PipeSegment.scad");
                    continue;
                }
                    
                // Loop through each segment and depending on its type and other properties assign it a suitable .scad file
                var pseudoCnt = 0;
                var filenamePrev = "";
                foreach (BaseSegment segment in component.Segments)
                {
                    if (segment.Filename != null)
                    {
                        segment.Filename = PathLocal + segment.Filename;
                        if (segment.Filename != filenamePrev)
                        {
                            pseudoCnt += 1;
                            filenamePrev = segment.Filename;
                        }
                    }

                    // Assign default template files for each segment
                    string fn;
                    if (!defaultTemplateFiles.TryGetValue(component.Type, out fn))
                        fn = "Cylinder.scad";

                    fn = PathMain + fn;

                    // Create template from default
                    segment.Template = new BaseTemplate(component, segment, fn);
                }

                

                // If segment count > 1 and exactly one local filename (file located in local components folder)
                //   - Assign component this filename
                //   - Remove segment filenames
                if (pseudoCnt == 1 && component.Segments.Count > 1)
                {
                    component.Filename = filenamePrev;

                    // Remove each filename if this is applied to the component level
                    foreach (BaseSegment segment in component.Segments)
                        segment.Filename = null;

                    // Update component template with information read from local file (if file doesn't exists it is created)
                    component.Template.ReadTemplateFile(component.Filename);  
                }
                // If not - replace default template with custom
                else
                {
                    foreach (BaseSegment segment in component.Segments)
                    {
                        if (segment.Filename == null)  // If no compo
                            continue;

                        // Update segment template with information read from local file (if file doesn't exists it is created from the default template)
                        segment.Template.ReadTemplateFile(segment.Filename);
                    }
                }
            }
        }

        /// <summary>
        /// Recursive method to 1) Assign coordinates to component, 2) Loop through its connections and run this method on these
        /// and 3) Loop through every connection in all components and see if they attach to this component
        /// </summary>
        /// <param name="CoordsToSet">The coordinates to assign to the component</param>
        /// <param name="Node">The node (1 or 2, inlet or outlet) where the coordinates apply</param>
        /// <param name="CurrentComponent">The component object</param>
        /// <param name="SegmentId">The segment id</param>
        public void AssignCoordinates(CoordsXYZ CoordsToSet, int Node, BaseComponent CurrentComponent, string SegmentId = "")
        {
            logger.Debug("- Assigning coordinates for '{0}'", CurrentComponent.TypeNameId);

            // If component coord is already set, quit
            if (CurrentComponent.CoordsIsSet == true)
            {
                logger.Debug("   - Coordinates already assigned");
                return;
            }
            else
            {
                CurrentComponent.SetCoordinates(new CoordsXYZ(CoordsToSet.X, CoordsToSet.Y, CoordsToSet.Z), Node, SegmentId);
                logger.Debug("   - Coordinates of {0} set to Coords1={1} and Coords2={2}", CurrentComponent.Name, CurrentComponent.Coords1.Repr(), CurrentComponent.Coords2.Repr());
            }
                       
            // Loop through its connections to see what it connects to - assign Coordinates to these
            foreach (BaseConnection connection in CurrentComponent.Connections())
            {
                var CoordsToSetToConnectingComponent = connection.SourceNode == 1 ? connection.SourceSegment.Coords1 : connection.SourceSegment.Coords2;
                if (connection.TargetSegment != null)
                {
                    logger.Debug("      - Connection {0} of '{1}': {2} is assigned coords={3}", -1, CurrentComponent.Name, connection.Repr(), CoordsToSetToConnectingComponent.Repr());
                    AssignCoordinates(CoordsToSetToConnectingComponent, connection.TargetNode, connection.TargetSegment.Parent, connection.TargetId);
                } 
            }

            // Loop through every junction/connecting component to see if it connects to current component, if it does - run assignCoordinates
            foreach (BaseComponent component in Components)
            {
                foreach (BaseConnection connection in component.Connections())
                {
                    int segmentIndex = CurrentComponent.IndexOf(connection.TargetSegment);
                    if (segmentIndex != -1)
                    {
                        var CoordsToSetToConnectingJunction = connection.TargetNode == 1 ? connection.TargetSegment.Coords1 : connection.TargetSegment.Coords2;   // CurrentComponent.GetCoordinates(connection.TargetNode, connection.TargetId);
                        AssignCoordinates(CoordsToSetToConnectingJunction, connection.SourceNode, component, connection.SourceSegment.UniqueId);
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

            foreach (BaseComponent component in Components)
            {
                s.WriteLine(component.WriteOutput());
            }
            s.WriteLine("}}");
            s.Close();
            f.Close();
            Console.WriteLine("File created successfully...");
        }

            
    }
    /// <summary>
    /// Logger class
    /// </summary>
    public class MyLogger
    {
        /// <summary>
        /// The level of what is to print out to screen.
        /// </summary>
        public int Level { get; set; }

        /*
        5. DEBUG
        4. INFO
        3. WARNING
        2. ERROR
        1. CRITICAL
        */

        /// <summary>
        /// Produces an error message
        /// </summary>
        /// <param name="format">A string.Format string</param>
        /// <param name="args">Arguments for the string.Format method</param>
        public void Error(string format, params object[] args)
        {
            if (Level >= 2) { Console.WriteLine(string.Format(format, args)); }
        }

        /// <summary>
        /// Produces an warning message 
        /// </summary>
        /// <param name="format">A string.Format string</param>
        /// <param name="args">Arguments for the string.Format method</param>
        public void Warning(string format, params object[] args)
        {
            if (Level >= 3) { Console.WriteLine(string.Format(format, args)); }
        }

        /// <summary>
        /// Produces an debug message
        /// </summary>
        /// <param name="format">A string.Format string</param>
        /// <param name="args">Arguments for the string.Format method</param>
        public void Debug(string format, params object[] args)
        {
            if (Level >= 5) { Console.WriteLine(string.Format(format, args)); }
        }

        /// <summary>
        /// Writes always visible message to console w/o line break
        /// </summary>
        /// <param name="format">A string.Format string</param>
        /// <param name="args">Arguments for the string.Format method</param>
        public void Write(string format, params object[] args)
        {
            Console.Write(string.Format(format, args));
        }

        /// <summary>
        /// Writes always visible message to console with line break
        /// </summary>
        /// <param name="format">A string.Format string</param>
        /// <param name="args">Arguments for the string.Format method</param>
        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
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
        /// <value>The name of the component. Equals the name of the first segment if multiple segments in component</value>
        public string Name { get { return Segments[0].Name; } }

        /// <value>The type of the component</value>
        public string Type { get { return Segments[0].Type; } }

        /// <value>A string representaton of the component</value>
        public string TypeNameId { get { return string.Format("Type={0}/Name={1}/Id={2}", Type, Name, Segments[0].UniqueId); } }

        /// <summary>
        /// If only one .scad-file is given for each segment in the component, this is the filename for the component.
        /// It is used to generate a aggregate .scad-file consisting of many segments
        /// </summary>
        /// <value>Empty string if no filename is given or component consists of many .scad-files</value>
        public string Filename { get; set; }

        public BaseTemplate Template { get; set; }

        /// <summary>A list containing the segments in the component</summary>
        public List<BaseSegment> Segments { get; set; }

        /// <summary>True</summary>
        /// <value>True if coordinates is set, false otherwise</value>
        public bool CoordsIsSet { get; set; }

        /// <summary>The coordinates of the inlet (node 1 of segment 0) of the component</summary>
        /// <value></value>
        public CoordsXYZ Coords1 { get { return Segments[0].Coords1; } }

        /// <summary>The coordinates of the inlet (node 2 of segment N) of the component</summary>
        /// <value></value>
        public CoordsXYZ Coords2 { get { return Segments[Segments.Count - 1].Coords2; } }

        /// <param name="InputString">A semi colon separated string with segment input for the first segment of the component</param>
        public BaseComponent(string InputString)
        {
            Segments = new List<BaseSegment>();
            var firstSegment = new BaseSegment(InputString);
            Segments.Add(firstSegment);
        }

        /// <param name="FirstSegment">A segment object for the first segment of the component</param>
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
        public void SetCoordinates(CoordsXYZ NewCoords, int Node, string SegmentId = "")
        {
            int segmentIndex = IndexOf(SegmentId);

            if (segmentIndex == -1)
            {
                Console.WriteLine(string.Format("SegmentId {0} not found in {1}", SegmentId, Name));
                return;
            }

            // Assign coordinates to segment with ID 'SegmentId'
            Segments[segmentIndex].SetCoordinates(new CoordsXYZ(NewCoords.X, NewCoords.Y, NewCoords.Z), Node);

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
        /// <example>
        /// <code>
        /// // Loops through every connection in each component
        /// foreach (BaseComponent component in Components)
        /// {
        ///     foreach (BaseConnection connection in component.Connections())
        ///     {
        ///         Console.Writeline(string.Format("Connection: {0}", connection.Repr()));
        ///     }
        /// }
        /// </code>
        /// </example>
        public IEnumerable<BaseConnection> Connections()
        {
            foreach (BaseSegment segment in Segments)
            {
                foreach (BaseConnection connection in segment.Connections)
                {
                    yield return connection;
                }
            }
        }

        /// <summary>
        /// Depending on what type of component, write out the OpenSCAD input for that component
        /// </summary>
        /// <returns>A string with output</returns>
        /// <see cref="ComponentOutput"/>
        public string WriteOutput()
        {
            return Template.OutputComponent(IndentLevel: 1);
        }
    }

    /// <summary>
    /// Class that reads and stores the segment properties (one line of input). A segment is a subset of a component that contains one or more segments.
    /// </summary>
    public class BaseSegment
    {
        private string name;

        /// <summary>The name of the segment</summary>
        /// <value>Name</value>
        public string Name
        {
            get { return name; }
            set
            {
                // If name contains a .scad extension, add to filename
                if (value.Right(5) == ".scad")
                {
                    Filename = value;
                    name = value.Substring(0, value.Length - 5);
                    Console.WriteLine("VALUE IS: " + Name);
                    Console.WriteLine("VALUE IS: " + Filename);
                }

            }
        }

        /// <summary>The unique id of the segment. Used to establish connections.</summary>
        /// <value>Unique Id</value>
        public string UniqueId { get; set; }
        
        /// <summary>The type of the segment. Pipe, valve etc.</summary>
        /// <value>Type</value>
        public string Type { get; set; }

        /// <summary>
        /// The filename of the .scad file to be used as a main template. Located in the a subfolder to the input file (usually named 'Components')
        /// The file contains OpenSCAD input for a segment oriented in the positive x-axis.
        /// </summary>
        /// <value>A absolute path to the .scad file. Null if not set.</value>
        public string Filename { get; set; }

        /// <summary>
        /// The filename of the .scad file to be used as a main template. Located in the 'Components\'-directory of the .exe location.
        /// </summary>
        /// <value>A absolute path to the .scad file.</value>
        public string TemplateMain { get; set; }

        public BaseTemplate Template { get; set; }

        /// <summary>The length of the segment in meters</summary>
        /// <value>Length</value>
        public double Length { get; set; }

        /// <summary>The vertical angle of the segment. Between -90 and +90 degrees.</summary>
        /// <value>Vertical angle</value>
        public double AngleVertical { get; set; }

        /// <summary>The azimuthal angle of the segment. Between 0 and 360 degrees. 0 degrees is towards the +x-axis, 90 degrees is towards +y-axis and so on.</summary>
        /// <value>Azimuthal angle</value>
        public double AngleAzimuthal { get; set; }

        /// <summary>Discrete x-coordinate change over the segment (in addition to that given by the length and the angles)</summary>
        /// <value>Additional change in x-direction</value>
        public double Dx { get; set; }

        /// <summary>Discrete y-coordinate change over the segment (in addition to that given by the length and the angles)</summary>
        /// /// <value>Additional change in y-direction</value>
        public double Dy { get; set; }

        /// <summary>Discrete z-coordinate change over the segment (in addition to that given by the length and the angles)</summary>
        /// /// <value>Additional change in z-direction</value>
        public double Dz { get; set; }

        /// <summary>The outer diameter in millimeters</summary>
        /// <value>Outer diameter [mm]</value>
        public double DiameterOuter { get; set; }

        /// <summary>The wall thickness in millimeters</summary>
        /// <value>Wall thickness [mm]</value>
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
        /// Constructor of a segment. Takes input in the form of a semi colon separated string.
        /// </summary>
        /// <param name="InputString"></param>
        public BaseSegment(string InputString)
        {
            string[] inputWords = InputString.Split(';');
            Coords1 = new CoordsXYZ();
            Coords2 = new CoordsXYZ();
            Connections = new List<BaseConnection>();

            if (inputWords.Length >= 17)
            {
                Type = inputWords[0];
                UniqueId = inputWords[1];
                Length = Convert.ToDouble(inputWords[2]);
                AngleVertical = Convert.ToDouble(inputWords[3]);
                AngleAzimuthal = Convert.ToDouble(inputWords[4]);
                Dx = Convert.ToDouble(inputWords[5]);
                Dy = Convert.ToDouble(inputWords[6]);
                Dz = Convert.ToDouble(inputWords[7]);
                DiameterOuter = Convert.ToDouble(inputWords[8])*0.001;
                WallThickness = Convert.ToDouble(inputWords[9])*0.001;
                Name = inputWords[10];

                Connections.Add(new BaseConnection(this, 1, inputWords[11], inputWords[12]));
                Connections.Add(new BaseConnection(this, 2, inputWords[13], inputWords[14]));

                double i;
                if (!Double.TryParse(inputWords[15], out i))
                    i = -1;
                Parameter1 = i;

                if (!Double.TryParse(inputWords[16], out i))
                    i = -1;
                Parameter2 = i;
            }
        }

        /// <summary>
        /// Sets the segment coordinates
        /// </summary>
        /// <param name="NewCoords">The new coordinates to set to segment</param>
        /// <param name="Node">The location (node 1 or 2, inlet or outlet) where the coordinates apply</param>
        public void SetCoordinates(CoordsXYZ NewCoords, int Node)
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

        /// <summary>The segment object of the target</summary>
        public BaseSegment TargetSegment { get; set; }

        /// <summary>The segment object of the source</summary>
        public BaseSegment SourceSegment { get; set; }

        /// <value>Returns true if connection is made</value>
        public bool ConnectionMade
        {
            get
            {
                if (TargetSegment == null && TargetId != "-")
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
        public BaseConnection(BaseSegment SourceSegment, int SourceNode,string TargetId, int TargetNode)
        {
            this.TargetId = TargetId;
            this.TargetNode = TargetNode;
            this.SourceNode = SourceNode;
            this.SourceSegment = SourceSegment;
        }

        /// <summary>
        /// Constructor for a connection object
        /// </summary>
        /// <param name="SourceSegment">The segment object of the source</param>
        /// <param name="SourceNode">The node where the connection attaches on the source segment</param>
        /// <param name="TargetId">The unique id of the target segment</param>
        /// <param name="TargetNode">The node where the connection attaches on the target segment</param>
        public BaseConnection(BaseSegment SourceSegment, int SourceNode, string TargetId, string TargetNode)
        {
            this.TargetId = TargetId;
            int i;
            if (!Int32.TryParse(TargetNode, out i))
                i = -1;
            
            this.TargetNode = i;
            this.SourceNode = SourceNode;
            this.SourceSegment = SourceSegment;
        }

        /// <summary>
        /// Returns a string representation of the connection object for debug purpose
        /// </summary>
        /// <returns>A string</returns>
        public string Repr() { return string.Format("[SourceName={0},SourceId={1},SourceNode={2},TargetId={3},TargetNode={4}]", SourceSegment.Name,SourceSegment.UniqueId, SourceNode, TargetSegment.UniqueId, TargetNode); }
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
            _readText = readSegment.ToString();
        }

        
        public string OutputComponent(int IndentLevel = 0)
        {
            var output = new StringBuilder();

            // If no segment is given - return output for whole component
            output.AppendFormat("// Type={1}, Name={2}, Id={3}{0}", Environment.NewLine, Component.Type, Component.Name, Component.Segments[0].UniqueId);
            output.AppendFormat("color(colorValve){0}", Environment.NewLine);
            output.AppendFormat("translate({1}){0}", Environment.NewLine, Component.Coords1.Repr());
            output.AppendFormat("{{ {0}", Environment.NewLine);

            if (_readText != "")
            {
                var componentRead = _readText;
                componentRead = componentRead.Insert(0, new string(' ', 4));
                componentRead = componentRead.Replace(Environment.NewLine, Environment.NewLine + new String(' ', 4));  // Add 1 level of indent
                output.AppendLine(componentRead); 
            }
            else
            {
                foreach (BaseSegment segment in Component.Segments)
                    output.AppendLine(segment.Template.OutputSegment(1));
            }
            output.AppendFormat("}} {0}", Environment.NewLine);

            // Add requested indent before every line
            output.Insert(0, new string(' ', 4*IndentLevel));
            output.Replace(Environment.NewLine, Environment.NewLine + new String(' ', 4*IndentLevel));

            return ReplaceKeywords(output.ToString());
        }

        public string OutputSegment(int IndentLevel = 0)
        {
            var output = new StringBuilder();

            output.AppendFormat("translate({1}){0}", Environment.NewLine, Segment.Coords1.Repr(Component.Coords1));
            output.AppendFormat("rotate([0,{1},{2}]){0}", Environment.NewLine, -Segment.AngleVertical, Segment.AngleAzimuthal);
            output.AppendFormat("{{ {0}", Environment.NewLine);

            var segmentRead = _readText;
            segmentRead = segmentRead.Insert(0, new string(' ', 4));
            segmentRead = segmentRead.Replace(Environment.NewLine, Environment.NewLine + new String(' ', 4));  // Add 1 level of indent

            output.AppendLine(segmentRead);
            output.AppendFormat("}} {0}", Environment.NewLine);
            
            // Add requested indent before every line
            output.Insert(0, new string(' ', 4 * IndentLevel));
            output.Replace(Environment.NewLine, Environment.NewLine + new String(' ', 4 * IndentLevel));

            return ReplaceKeywords(output.ToString());
        }

        public void WriteTemplateFile(string Filename)
        {
            FileStream f = new FileStream(Filename, FileMode.Create);
            StreamWriter s = new StreamWriter(f);
            
            if (Segment == null)
            {
                Console.WriteLine(string.Format("- Creating multi segment template file = {0}", Filename));
                s.WriteLine("$fn=50;  // REMOVE");
                s.WriteLine("LENGTH = 1;  // REMOVE");
                s.WriteLine("DO = 1;  // REMOVE");
                foreach (BaseSegment segment in Component.Segments)
                {
                    s.WriteLine(segment.Template.OutputSegment());
                }
            }
            else
            {
                Console.WriteLine(string.Format("- Creating single segment template file = {0}", Filename));
                s.WriteLine("$fn=50;  // REMOVE");
                s.WriteLine("LENGTH = 1;  // REMOVE");
                s.WriteLine("DO = 1;  // REMOVE");
                s.WriteLine(_readText);
            }
            
            s.Close();
            f.Close();
        }

        private string ReplaceKeywords(string str)
        {
            var strOut = str;

            string length, diameterOuter, wallThickness, parameter1;
            if (Segment != null)
            {
                length = string.Format("{0}", Segment.Length);
                diameterOuter = string.Format("{0}", Segment.DiameterOuter);
                wallThickness = string.Format("{0}", Segment.WallThickness);
                parameter1 = string.Format("{0}", Segment.Parameter1);

                strOut = strOut.Replace("(LENGTH)", length);
                strOut = strOut.Replace("(DO)", diameterOuter);
                strOut = strOut.Replace("(WALLTHICKNESS)", wallThickness);
                strOut = strOut.Replace("(PARAM1)", parameter1);
            }

            var ind = 0;
            var nodes = "[";
            var bendradius = "[";
            foreach (BaseSegment segment in Component.Segments)
            {
                length = string.Format("{0}", segment.Length);
                diameterOuter = string.Format("{0}", segment.DiameterOuter);
                wallThickness = string.Format("{0}", segment.WallThickness);
                parameter1 = string.Format("{0}", segment.Parameter1);

                strOut = strOut.Replace(string.Format("(LENGTH_{0})", ind), length);
                strOut = strOut.Replace(string.Format("(DO_{0})", ind), diameterOuter);
                strOut = strOut.Replace(string.Format("(WALLTHICKNESS_{0})", ind), diameterOuter);
                strOut = strOut.Replace(string.Format("(PARAM1_{0})", ind), diameterOuter);

                nodes += segment.Coords1.Repr(Component.Coords1) + ",";
                nodes += (ind == Component.Segments.Count - 1) ? segment.Coords2.Repr(Component.Coords1) + "]" : "";
                bendradius += string.Format("{0}", segment.Parameter1 * segment.DiameterOuter);
                bendradius += (ind == Component.Segments.Count - 1) ? "]" : ",";
                ind += 1;
            }

            // If pipe component
            diameterOuter = string.Format("{0}", Component.Segments[0].DiameterOuter);
            wallThickness = string.Format("{0}", Component.Segments[0].WallThickness);
            strOut = strOut.Replace("(DO)", diameterOuter);
            strOut = strOut.Replace("(WALLTHICKNESS)", wallThickness);
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
}




namespace Bloat
{
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
