namespace Tools.Operators {
    class StandardMinifyImport : FileOperator {
        protected string Path { get; }
        protected MinifyOptions Options { get; }
        protected Dictionary<string, string> Key { get; }
        protected bool IsStandard { get; }
        private List<string> Stdi { get; }
        private List<int> Current { get; }
        public StandardMinifyImport(string path, Librarian librarian, MinifyOptions options, Dictionary<string, string> key, List<int> current, List<string> stdi, bool isStandard = true) : base(librarian, -1, -1) {
            this.Path = path;
            this.Options = options;
            this.Key = key;
            this.IsStandard = isStandard;
            this.Stdi = stdi;
            this.Current = current;
        }
        protected virtual string GetPath(Stack Stack) {
            return Path;
        }
        public override IValue Run(Stack Stack) {
            string realPath = GetPath(Stack);
            if(Librarian.CurrentlyImporting.Contains(realPath)) {
                throw new RadishException($"Circular dependency detected at {realPath}!", Row, Col);
            }
            CountingReader reader;
            try {
                reader = new CountingReader(realPath);
            } catch {
                throw new RadishException($"Could not find file {realPath}", Row, Col);
            }
            Minifier minifier = new Minifier(reader, Librarian, Options, false, IsStandard, Key, Current, Stdi);
            string previous = RadishException.FileName;
            RadishException.FileName = realPath;
            Librarian.CurrentlyImporting.Push(realPath);
            minifier.ParseScope();
            string returning = minifier.Output;
            RadishException.FileName = previous;
            Librarian.CurrentlyImporting.Pop();
            
            return new Values.StringLiteral(returning);
        }
        public override string Print() {
            return $"(minify-standard-import {Path})";
        }
    }
}