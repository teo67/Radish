namespace Tools.Operators {
    class Decrement : Operator {
        private IOperator Stored { get; }
        public Decrement(IOperator stored, int row, int col) : base(row, col) {
            this.Stored = stored;
        }
        public override IValue Run(Stack Stack) {
            IValue result = Stored._Run(Stack);
            result.Var = new Values.NumberLiteral(result.Number - 1);
            return result;
        }
        public override string Print() {
            return $"({Stored.Print()}--)";
        }
    }
}