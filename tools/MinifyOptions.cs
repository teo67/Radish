namespace Tools {
    class MinifyOptions {
        public bool CombinerOptimizations { get; }
        public bool IncrementDecrementOptimizations { get; }
        public bool ShortenKeywords { get; }
        public bool FunctionOptimizations { get; }
        public bool VariableNames { get; }
        public bool PropertyNames { get; }
        public bool IncludeImports { get; }
        public MinifyOptions(bool[] options) {
            this.CombinerOptimizations = options[0];
            this.IncrementDecrementOptimizations = options[1];
            this.ShortenKeywords = options[2];
            this.FunctionOptimizations = options[3];
            this.VariableNames = options[4];
            this.PropertyNames = options[5];
            this.IncludeImports = options[6];
        }
    }
}