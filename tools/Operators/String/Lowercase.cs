namespace Tools.Operators {
    class Lowercase : IOperator {
        private IOperator Stored { get; }
        private IValue Str { get; }
        public Lowercase(IOperator stored, IValue str) {
            this.Stored = stored;
            this.Str = str;
        }
        public IValue Run() {
            return new Values.StringLiteral(Stored.Run().String.ToLower(), Str);
        }
        public string Print() {
            return $"(\"{Stored.Print()}\" to lower case)";
        }
    }
}