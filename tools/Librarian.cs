namespace Tools { // adds basic prototypes to call stack
    class Librarian {
        private Values.Variable AddProto(string name, List<Values.Variable> layer, bool _useProto = true) {
            Values.Variable returning = new Values.Variable(name, new Values.ObjectLiteral(new List<Values.Variable>(), useProto: _useProto));
            layer.Add(returning);
            return returning;
        }
        public Dictionary<string, IValue> Imports { get; }
        public StackNode FirstLayer { get; }
        public List<string> Standard { get; }
        private string? PathToLibrary { get; }
        private Stack ImportStack { get; }
        private List<Values.Variable> StandardSpecials { get; }
        public Stack<string> CurrentlyImporting { get; }
        public Librarian(bool uselib = true) {
            CurrentlyImporting = new Stack<string>();
            Imports = new Dictionary<string, IValue>();
            List<Values.Variable> layer = new List<Values.Variable>();
            Values.ObjectLiteral.Proto = AddProto("Object", layer, false);
            Values.NumberLiteral.Proto = AddProto("Number", layer);
            Values.StringLiteral.Proto = AddProto("String", layer);
            Values.ObjectLiteral.ArrayProto = AddProto("Array", layer);
            Values.BooleanLiteral.Proto = AddProto("Boolean", layer);
            Values.FunctionLiteral.Proto = AddProto("Function", layer);
            FirstLayer = new StackNode(layer);
            Standard = new List<string>();
            ImportStack = new Stack(FirstLayer);
            PathToLibrary = null;
            StandardSpecials = new List<Values.Variable>();
            if(uselib) {
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
                
                foreach(string path in fullPaths) {
                    string? dirName = Path.GetFileName(path);
                    if(dirName != null && dirName != "PROTOTYPES") { // prototypes gets executed immediately
                        Standard.Add(dirName);
                    }
                }
                StandardSpecials.Add(new Values.Variable("OUTPUT", new Values.FunctionLiteral(ImportStack, new List<string>() { "input" }, new List<IOperator?>() { null }, false, new Operators.Output(new Operators.Reference("input", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("LOG", new Values.FunctionLiteral(ImportStack, new List<string>() { "loginput", "logbase" }, new List<IOperator?>() { null, null }, false, new Operators.Log(new Operators.Reference("loginput", -1, -1, this), new Operators.Reference("logbase", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("ARRLEN", new Values.FunctionLiteral(ImportStack, new List<string>() { "arrinput" }, new List<IOperator?>() { null }, false, new Operators.ArrayLength(new Operators.Reference("arrinput", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("STRLEN", new Values.FunctionLiteral(ImportStack, new List<string>() { "strinput" }, new List<IOperator?>() { null }, false, new Operators.StringLength(new Operators.Reference("strinput", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("DELETE", new Values.FunctionLiteral(ImportStack, new List<string>() { "delobj", "delinput" }, new List<IOperator?>() { null, null }, false, new Operators.ObjectUproot(new Operators.Reference("delobj", -1, -1, this), new Operators.Reference("delinput", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("CALL", new Values.FunctionLiteral(ImportStack, new List<string>() { "caller", "callinput", "callthis" }, new List<IOperator?>() { null, null, null }, false, new Operators.FunctionCall(new Operators.Reference("caller", -1, -1, this), new Operators.Reference("callinput", -1, -1, this), -1, -1, new Operators.Reference("callthis", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("FILEREAD", new Values.FunctionLiteral(ImportStack, new List<string>() { "readfile" }, new List<IOperator?>() { null }, false, new Operators.FileRead(new Operators.Reference("readfile", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("FILEWRITE", new Values.FunctionLiteral(ImportStack, new List<string>() { "writefile", "writeinput" }, new List<IOperator?>() { null, null }, false, new Operators.FileWrite(new Operators.Reference("writefile", -1, -1, this), new Operators.Reference("writeinput", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("FILEEXISTS", new Values.FunctionLiteral(ImportStack, new List<string>() { "existsfile" }, new List<IOperator?>() { null }, false, new Operators.FileExists(new Operators.Reference("existsfile", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("FILECREATE", new Values.FunctionLiteral(ImportStack, new List<string>() { "createfile" }, new List<IOperator?>() { null }, false, new Operators.FileCreate(new Operators.Reference("createfile", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("FILEDELETE", new Values.FunctionLiteral(ImportStack, new List<string>() { "deletefile" }, new List<IOperator?>() { null }, false, new Operators.FileDelete(new Operators.Reference("deletefile", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("DIRECTORYEXISTS", new Values.FunctionLiteral(ImportStack, new List<string>() { "existsdirectory" }, new List<IOperator?>() { null }, false, new Operators.DirectoryExists(new Operators.Reference("existsdirectory", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("DIRECTORYCREATE", new Values.FunctionLiteral(ImportStack, new List<string>() { "createdirectory" }, new List<IOperator?>() { null }, false, new Operators.DirectoryCreate(new Operators.Reference("createdirectory", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("DIRECTORYDELETE", new Values.FunctionLiteral(ImportStack, new List<string>() { "deletedirectory" }, new List<IOperator?>() { null }, false, new Operators.DirectoryDelete(new Operators.Reference("deletedirectory", -1, -1, this)), "Standard Library")));
                StandardSpecials.Add(new Values.Variable("DIRECTORYREAD", new Values.FunctionLiteral(ImportStack, new List<string>() { "readdir" }, new List<IOperator?>() { null }, false, new Operators.DirectoryRead(new Operators.Reference("readdir", -1, -1, this)), "Standard Library")));
                Lookup("PROTOTYPES", -1, -1); // we lookup prototypes at the beginning to add properties to literal classes
                //this will directly edit the first layer of the stack
            } else {
                layer.Add(new Values.Variable("holler", new Values.FunctionLiteral(ImportStack, new List<string>() { "input" }, new List<IOperator?>() { null }, false, new Operators.Output(new Operators.Reference("input", -1, -1, this)), "Standard Library")));
                // add a basic holler function j ust so radish is still usable
            }
        }

        public IValue? Import(string path) {
            if(Imports.ContainsKey(path)) {
                //Console.WriteLine("successful import reduction");
                return Imports[path];
            }
            //Console.WriteLine( "importing for the first time");
            return null;
        }

        public IValue SpecialImport(string varName) {
            foreach(Values.Variable vari in StandardSpecials) {
                if(vari.Name == varName) {
                    //Console.WriteLine("found variable used in standard specials");
                    return vari;
                }
            }
            throw new RadishException($"No special variable found for {varName}!");
        }

        public IValue Lookup(string varName, int row, int col) {
            if(PathToLibrary == null) {
                throw new RadishException("Radish is currently being run without a standard library!", row, col);
            }
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
            Operations operations = new Operations(reader, false, true, this); // only case in which it's okay to set last arg to true
            string previous = RadishException.FileName;
            RadishException.FileName = path;
            IValue returned = operations.ParseScope().Run(operations.stack);
            RadishException.FileName = previous;
            if(returned.Default == BasicTypes.RETURN) { 
                returned = returned.Function(new List<Values.Variable>(), null);
            }
            if(varName != "PROTOTYPES") {
                Imports.Add(path, returned);
            }
            return returned;
        }
    }
}