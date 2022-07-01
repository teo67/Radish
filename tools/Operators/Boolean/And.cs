namespace Tools.Operators {
    class And : SimpleOperator {
        public And(IOperator left, IOperator right, int row, int col) : base(left, right, "&&", row, col) { }
        public override IValue Run(Stack Stack) {
            return new Values.BooleanLiteral(Left._Run(Stack).Boolean && Right._Run(Stack).Boolean);
        }
    }
}