namespace Tools.Operators {
    class Import : VariableOperator {
        private IOperator FileName { get; }
        private string Path { get; }
        public Import(Stack stack, IOperator fileName, int row, int col, string path) : base(stack, row, col) {
            this.FileName = fileName;
            this.Path = path;
        }
        public override IValue Run() {
            string name = FileName._Run().String;
            if(!name.EndsWith(".rdsh")) {
                name += ".rdsh";
            }
            string modPath = Path;
            while(name.StartsWith("../")) {
                int lastI = modPath.LastIndexOf('/');
                if(lastI == -1) {
                    throw new RadishException($"File {name} escapes an invalid number of folders!", Row, Col);
                }
                modPath = modPath.Substring(0, lastI);
                name = name.Substring(3);
            }
            CountingReader reader;
            try {
                reader = new CountingReader(modPath + "/" + name);
            } catch {
                throw new RadishException($"Could not find file {name}", Row, Col);
            }
            Operations operations = new Operations(reader, false);
            RadishException.Append($"in {modPath + "/" + name}", -1, -1);
            IValue returned = operations.ParseScope().Run();
            RadishException.Pop();
            return (returned.Default == BasicTypes.RETURN ? returned.Function(new List<Values.Variable>()) : returned);
        }
    }
}