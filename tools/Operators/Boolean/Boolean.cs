namespace Tools.Operators {
    class Boolean : Operator {
        private bool Stored { get; }
        public Boolean(bool stored, int row, int col) : base(row, col) {
            this.Stored = stored;
        }
        public override IValue Run() {
            return new Values.BooleanLiteral(Stored);
        }
        public override string Print() {
            return (Stored) ? "yes" : "no";
        }
    }
}