namespace Tools {
    class MinifyOptions {
        public bool ShortenKeywords { get; }
        public bool FunctionOptimizations { get; }
        public bool VariableNames { get; }
        public bool IncludeImports { get; }
        public MinifyOptions(bool[] options) {
            this.ShortenKeywords = options[0];
            this.FunctionOptimizations = options[1];
            this.VariableNames = options[2];
            this.IncludeImports = options[3];
        }
    }
}