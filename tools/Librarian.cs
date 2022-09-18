namespace Tools { // adds basic prototypes to call stack
    class Librarian {
        private Values.Variable AddProto(string name, Dictionary<string, Values.Variable> layer, bool _useProto = true) {
            Values.Variable returning = new Values.Variable(new Values.ObjectLiteral(new Dictionary<string, Values.Variable>(), useProto: _useProto));
            layer.Add(name, returning);
            return returning;
        }
        public Dictionary<string, IValue> Imports { get; }
        public StackNode FirstLayer { get; }
        public List<string> Standard { get; }
        private string? PathToLibrary { get; }
        private Dictionary<string, IOperator> StandardSpecials { get; }
        public Stack<string> CurrentlyImporting { get; }
        public Librarian(bool uselib = true) {
            CurrentlyImporting = new Stack<string>();
            Imports = new Dictionary<string, IValue>();
            Dictionary<string, Values.Variable> layer = new Dictionary<string, Values.Variable>();
            Values.ObjectLiteral.Proto = AddProto("Object", layer, false);
            Values.NumberLiteral.Proto = AddProto("Number", layer);
            Values.StringLiteral.Proto = AddProto("String", layer);
            Values.ObjectLiteral.ArrayProto = AddProto("Array", layer);
            Values.BooleanLiteral.Proto = AddProto("Boolean", layer);
            Values.FunctionLiteral.Proto = AddProto("Function", layer);
            Values.PolyLiteral.Proto = AddProto("Poly", layer);
            FirstLayer = new StackNode(layer);
            Standard = new List<string>();
            PathToLibrary = null;
            StandardSpecials = new Dictionary<string, IOperator>();
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
                StandardSpecials.Add("OUTPUT", new Operators.Output(this));
                StandardSpecials.Add("LOG", new Operators.Log(this));
                StandardSpecials.Add("ARRLEN", new Operators.ArrayLength(this));
                StandardSpecials.Add("STRLEN", new Operators.StringLength(this));
                StandardSpecials.Add("DELETE", new Operators.ObjectUproot(this));
                StandardSpecials.Add("CALL", new Operators.SpecialCall(this));
                StandardSpecials.Add("FILEREAD", new Operators.FileRead(this));
                StandardSpecials.Add("FILEWRITE", new Operators.FileWrite(this));
                StandardSpecials.Add("FILEEXISTS", new Operators.FileExists(this));
                StandardSpecials.Add("FILECREATE", new Operators.FileCreate(this));
                StandardSpecials.Add("FILEDELETE", new Operators.FileDelete(this));
                StandardSpecials.Add("DIRECTORYEXISTS", new Operators.DirectoryExists(this));
                StandardSpecials.Add("DIRECTORYCREATE", new Operators.DirectoryCreate(this));
                StandardSpecials.Add("DIRECTORYDELETE", new Operators.DirectoryDelete(this));
                StandardSpecials.Add("DIRECTORYREAD", new Operators.DirectoryRead(this));
                StandardSpecials.Add("DOFILE", new Operators.DoFile(this));
                StandardSpecials.Add("SIN", new Operators.Sine(this));
                StandardSpecials.Add("COS", new Operators.Cosine(this));
                StandardSpecials.Add("TAN", new Operators.Tangent(this));
                StandardSpecials.Add("ASIN", new Operators.Arcsine(this));
                StandardSpecials.Add("ACOS", new Operators.Arccosine(this));
                StandardSpecials.Add("ATAN", new Operators.Arctangent(this));
                StandardSpecials.Add("WRITE", new Operators.Write(this));
                StandardSpecials.Add("CLEAR", new Operators.Clear());
                StandardSpecials.Add("READ", new Operators.Read());
                StandardSpecials.Add("NEXT", new Operators.Next());
                StandardSpecials.Add("XOR128SHIFTPLUS", new Operators.XOrShift128Plus(this));
                StandardSpecials.Add("OBJGET", new Operators.Objget(this));
                StandardSpecials.Add("KEYS", new Operators.Keys(this));
                StandardSpecials.Add("VALUES", new Operators._Values(this));
                StandardSpecials.Add("FUNCOPY", new Operators.CopyFunction(this));
                StandardSpecials.Add("EXECUTE", new Operators.Execute(this));
                StandardSpecials.Add("TOJSON", new Operators.ToJSON(this));
                StandardSpecials.Add("FROMJSON", new Operators.FromJSON(this));
                StandardSpecials.Add("GENERATEQR", new Operators.GenerateQR(this));
                StandardSpecials.Add("YEAR", new Operators.Current(Operators.DateType.YEAR, this));
                StandardSpecials.Add("MONTH", new Operators.Current(Operators.DateType.MONTH, this));
                StandardSpecials.Add("DAY", new Operators.Current(Operators.DateType.DAY, this));
                StandardSpecials.Add("HOUR", new Operators.Current(Operators.DateType.HOUR, this));
                StandardSpecials.Add("MINUTE", new Operators.Current(Operators.DateType.MINUTE, this));
                StandardSpecials.Add("SECOND", new Operators.Current(Operators.DateType.SECOND, this));
                StandardSpecials.Add("MILLISECOND", new Operators.Current(Operators.DateType.MILLISECOND, this));
                StandardSpecials.Add("NOW", new Operators.Now());
                
                StandardSpecials.Add("GENERATEBMP", new Operators.GenerateBMP(this));
                StandardSpecials.Add("RENDERBMP", new Operators.RenderBMP(this));
                StandardSpecials.Add("ITERATEBMP", new Operators.IterateBMP(this));
                StandardSpecials.Add("EDITPALLETTE", new Operators.EditPallette(this));
                StandardSpecials.Add("DRAWRECTANGLE", new Operators.DrawRectangle(this));
                StandardSpecials.Add("DRAWLINE", new Operators.DrawLine(this));
                StandardSpecials.Add("DRAWELLIPSE", new Operators.DrawEllipse(this));
                StandardSpecials.Add("DRAWPOINT", new Operators.DrawPoint(this));
                StandardSpecials.Add("FILLTRIANGLE", new Operators.FillNoHoles(this));
                StandardSpecials.Add("PRINTBMP", new Operators.PrintBMP(this));

                StandardSpecials.Add("POLYADDSTRING", new Operators.PolyAddString(this));
                StandardSpecials.Add("POLYMULTIPLYSTRING", new Operators.PolyMultiplyString(this));
                StandardSpecials.Add("ROUND", new Operators.Round(this));
                Lookup("PROTOTYPES", -1, -1); // we lookup prototypes at the beginning to add properties to literal classes
                //this will directly edit the first layer of the stack
            } else {
                layer.Add("holler", new Values.Variable(new Values.FunctionLiteral(new Stack(new StackNode(new Dictionary<string, Values.Variable>())), new List<string>() { "0" }, new List<IOperator?>() { null }, false, new Operators.Output(this), "Standard Library")));
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

        public IOperator SpecialImport(string varName) {
            IOperator? vari = null;
            bool gotten = StandardSpecials.TryGetValue(varName, out vari);
            if(gotten && vari != null) {
                return vari;
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
                returned = returned.Function(new List<IValue>());
            }
            if(varName != "PROTOTYPES") {
                Imports.Add(path, returned);
            }
            return returned;
        }
    }
}