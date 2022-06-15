namespace Tools { // adds basic prototypes to call stack
    class Librarian {
        public static Dictionary<string, IValue> Imports { get; }
        public static StackNode FirstLayer { get; }
        public static List<string> Standard { get; }
        private static string PathToLibrary { get; }
        private static Stack ImportStack { get; }
        private static List<Values.Variable> StandardSpecials { get; }
        static Librarian() {
            Imports = new Dictionary<string, IValue>();
            List<Values.Variable> layer = new List<Values.Variable>();
            IValue Object = new Values.ObjectLiteral(new List<Values.Variable>(), null);
            layer.Add(new Values.Variable("Object", Object));
            IValue Number = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            layer.Add(new Values.Variable("Number", Number));
            IValue String = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            layer.Add(new Values.Variable("String", String));
            IValue Array = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            layer.Add(new Values.Variable("Array", Array));
            IValue Boolean = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            layer.Add(new Values.Variable("Boolean", Boolean));
            IValue Function = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            layer.Add(new Values.Variable("Function", Function));
            FirstLayer = new StackNode(layer);
            
            DirectoryInfo? returned = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory);
            if(returned == null) {
                throw new RadishException("Radish is not running in a valid directory!", -1, -1);
            }
            returned = Directory.GetParent(returned.FullName);
            if(returned == null) {
                throw new RadishException("Radish is not installed correctly! Make sure that the bin folder is located in the Radish folder and contains an executable file.", -1, -1);
            }
            PathToLibrary = Path.Combine(returned.FullName, "lib");
            if(!Directory.Exists(PathToLibrary)) {
                throw new RadishException("The Radish standard library has not been installed properly! It should be located in the lib folder within Radish.", -1, -1);
            }
            string[] fullPaths = Directory.GetDirectories(PathToLibrary);
            Standard = new List<string>();
            foreach(string path in fullPaths) {
                string? dirName = Path.GetFileName(path);
                if(dirName != null && dirName != "PROTOTYPES") { // prototypes gets executed immediately
                    Standard.Add(dirName);
                }
            }
            ImportStack = new Stack(FirstLayer);

            StandardSpecials = new List<Values.Variable>() {
                new Values.Variable("OUTPUT", new Values.FunctionLiteral(ImportStack, new List<string>() { "input" }, new List<IOperator?>() { null }, new Operators.Output(new Operators.Reference(ImportStack, "input", -1, -1), ImportStack), Function))
            };

            Librarian.Lookup("PROTOTYPES", -1, -1); // we lookup prototypes at the beginning to add properties to literal classes
            //this will directly edit the first layer of the stack
        }

        public static IValue? Import(string path) {
            if(Imports.ContainsKey(path)) {
                //Console.WriteLine("successful import reduction");
                return Imports[path];
            }
            //Console.WriteLine( "importing for the first time");
            return null;
        }

        public static IValue? SpecialImport(string varName) {
            foreach(Values.Variable vari in StandardSpecials) {
                if(vari.Name == varName) {
                    //Console.WriteLine("found variable used in standard specials");
                    return vari;
                }
            }
            //Console.WriteLine($"No special variable found for {varName}!");
            return null;
        }

        public static IValue Lookup(string varName, int row, int col) {
            //Console.WriteLine("looking up...");
            string path = Path.Combine(PathToLibrary, varName).Replace('\\', '/') + "/main.rdsh";
            //Console.WriteLine($"path: {path}");
            IValue? already = Import(path);
            if(already != null) {
                //Console.WriteLine("standard value has already been parsed");
                return already;
            }
            CountingReader reader;
            try {
                reader = new CountingReader(path);
            } catch {
                throw new RadishException($"Could not find a standard library definition for {path}", row, col);
            }
            Operations operations = new Operations(reader, false, true); // only case in which it's okay to set last arg to true
            RadishException.Append($"in {path}", -1, -1);
            IValue returned = operations.ParseScope().Run();
            RadishException.Pop();
            if(returned.Default == BasicTypes.RETURN) { 
                returned = returned.Function(new List<Values.Variable>(), null);
            }
            if(varName != "PROTOTYPES") {
                Librarian.Imports.Add(path, returned);
            }
            return returned;
        }
    }
}