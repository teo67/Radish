namespace Tools.Operators {
    class MinifyImport : StandardMinifyImport {
        private IOperator FileName { get; }
        public MinifyImport(IOperator fileName, string path, Librarian librarian, MinifyOptions options, Dictionary<string, string> key, List<int> current, List<string> stdi, bool isStandard) : base(path, librarian, options, key, current, stdi, isStandard) {
            this.FileName = fileName;
        }
        protected override string GetPath(Stack Stack) {
            string realPath = Convert(FileName, Path, Stack);
            if(!realPath.EndsWith(".rdsh")) {
                realPath += ".rdsh";
            }
            return realPath;
        }
        public override string Print() {
            return $"(minify-import {FileName.Print()})";
        }
    }
}