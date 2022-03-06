namespace Tools.Operators {
    class Boolean : IOperator {
        private bool Stored { get; }
        public Boolean(bool stored) {
            this.Stored = stored;
        }
        public IValue Run() {
            return new Values.BooleanLiteral(Stored);
        }
        public string Print() {
            return (Stored) ? "true" : "false";
        }
    }
}