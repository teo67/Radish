namespace Tools.Operators {
    class MoreThanOrEquals : SimpleOperator {
        public MoreThanOrEquals(IOperator left, IOperator right, int row, int col) : base(left, right, ">=", row, col) { }
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            return new Values.BooleanLiteral(leftResult.Number >= rightResult.Number);
        }
    }
}