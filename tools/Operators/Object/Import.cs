namespace Tools.Operators {
    class Import : FileOperator {
        private Librarian Librarian { get; }
        private bool IsStandard { get; }
        private string Path { get; }
        public Import(IOperator fileName, int row, int col, string path, Librarian librarian, bool isStandard) : base(fileName, row, col) {
            this.Librarian = librarian;
            this.IsStandard = isStandard;
            this.Path = path;
        }
        public override IValue Run(Stack Stack) {
            string realPath = Convert(Path, Stack);
            if(!realPath.EndsWith(".rdsh")) {
                realPath += ".rdsh";
            }
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
                throw new RadishException($"Could not find file {realPath}", Row, Col);
            }
            Operations operations = new Operations(reader, false, IsStandard, Librarian);
            string previous = RadishException.FileName;
            RadishException.FileName = realPath;
            Librarian.CurrentlyImporting.Push(realPath);
            IValue returned = operations.ParseScope().Run(operations.stack);
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