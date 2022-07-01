namespace Tools.Operators {
    class Negative : Operator {
        private IOperator Stored { get; }
        public Negative(IOperator stored, int row, int col) : base(row, col) {
            this.Stored = stored;
        }
        public override IValue Run(Stack Stack) {
            IValue result = Stored._Run(Stack).Var;
            return new Values.NumberLiteral(result.Number * -1);
        }
        public override string Print() {
            return $"(-{Stored.Print()})";
        }
    }
}