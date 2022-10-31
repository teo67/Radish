namespace Tools.Operators {
    class MinifyImport : FileOperator {
        private IOperator FileName { get; }
        private string Path { get; }
        private MinifyOptions Options { get; }
        public MinifyImport(IOperator fileName, string path, Librarian librarian, MinifyOptions options) : base(librarian, -1, -1) {
            this.Path = path;
            this.Options = options;
            this.FileName = fileName;
        }
        public override IValue Run(Stack Stack) {
            string realPath = Convert(FileName, Path, Stack);
            if(!realPath.EndsWith(".rdsh")) {
                realPath += ".rdsh";
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
            Minifier minifier = new Minifier(reader, Librarian, Options, false);
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
            return $"(minify-import {FileName})";
        }
    }
}