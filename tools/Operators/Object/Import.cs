namespace Tools.Operators {
    class Import : Operator {
        private IOperator FileName { get; }
        private string Path { get; }
        private Librarian Librarian { get; }
        private bool IsStandard { get; }
        public Import(IOperator fileName, int row, int col, string path, Librarian librarian, bool isStandard) : base(row, col) {
            this.FileName = fileName;
            int lastI = path.LastIndexOf('/');
            if(lastI == -1) {
                throw new RadishException($"Invalid import path: {fileName}!", Row, Col);
            }
            this.Path = path.Substring(0, lastI);
            this.Librarian = librarian;
            this.IsStandard = isStandard;
        }
        public override IValue Run(Stack Stack) {
            string name = FileName._Run(Stack).String;
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
            string realPath = modPath + "/" + name;
            IValue? libd = Librarian.Import(realPath);
            if(libd != null) {
                return libd; // no need to import something twice
            }
            if(Librarian.CurrentlyImporting.Contains(realPath)) {
                throw new RadishException($"Circular dependency detected at {realPath}!", Row, Col);
            }
            CountingReader reader;
            try {
                reader = new CountingReader(realPath);
            } catch {
                throw new RadishException($"Could not find file {name}", Row, Col);
            }
            Operations operations = new Operations(reader, false, IsStandard, Librarian);
            string previous = RadishException.FileName;
            RadishException.FileName = realPath;
            Librarian.CurrentlyImporting.Push(realPath);
            IValue returned = operations.ParseScope().Run(Stack);
            RadishException.FileName = previous;
            Librarian.CurrentlyImporting.Pop();
            if(returned.Default == BasicTypes.RETURN) { 
                returned = returned.Function(new List<Values.Variable>(), null);
            }
            Librarian.Imports.Add(realPath, returned);
            return returned;
        }
        public override string Print() {
            return $"import {FileName})";
        }
    }
}