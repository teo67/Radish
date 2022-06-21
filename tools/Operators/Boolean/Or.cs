namespace Tools.Operators {
    class Or : SimpleOperator {
        public Or(IOperator left, IOperator right, int row, int col) : base(left, right, "||", row, col) { }
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            return new Values.BooleanLiteral(leftResult.Boolean || rightResult.Boolean);
        }
    }
}