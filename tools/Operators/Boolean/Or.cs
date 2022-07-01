namespace Tools.Operators {
    class Or : SimpleOperator {
        public Or(IOperator left, IOperator right, int row, int col) : base(left, right, "||", row, col) { }
        public override IValue Run(Stack Stack) {
            return new Values.BooleanLiteral(Left._Run(Stack).Boolean || Right._Run(Stack).Boolean);
        }
    }
}