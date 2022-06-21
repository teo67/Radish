namespace Tools.Operators {
    class Import : Operator {
        private IOperator FileName { get; }
        private string Path { get; }
        private Librarian Librarian { get; }
        public Import(IOperator fileName, int row, int col, string path, Librarian librarian) : base(row, col) {
            this.FileName = fileName;
            this.Path = path;
            this.Librarian = librarian;
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
            Operations operations = new Operations(reader, false, false, Librarian);
            RadishException.Append($"in {realPath}", -1, -1, false);
            Librarian.CurrentlyImporting.Push(realPath);
            IValue returned = operations.ParseScope().Run();
            RadishException.Pop();
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