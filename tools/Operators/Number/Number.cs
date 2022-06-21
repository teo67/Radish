namespace Tools.Operators {
    class Number : Operator {
        private double Stored { get; }
        public Number(double stored, int row, int col) : base(row, col) {
            this.Stored = stored;
        }
        public override IValue Run() {
            return new Values.NumberLiteral(Stored);
        }
        public override string Print() {
            return $"{Stored}";
        }
    }
}