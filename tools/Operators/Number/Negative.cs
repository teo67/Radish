namespace Tools.Operators {
    class Negative : Operator {
        private IOperator Stored { get; }
        public Negative(IOperator stored, int row, int col) : base(row, col) {
            this.Stored = stored;
        }
        public override IValue Run() {
            IValue result = Stored._Run().Var;
            return new Values.NumberLiteral(result.Number * -1);
        }
        public override string Print() {
            return $"(-{Stored.Print()})";
        }
    }
}