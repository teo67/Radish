namespace Tools.Operators {
    class String : IOperator {
        private string Stored { get; }
        public String(string stored) {
            this.Stored = stored;
        }
        public IValue Run() {
            return new Values.StringLiteral(Stored);
        }
        public string Print() {
            return Stored;
        }
    }
}