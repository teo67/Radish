namespace Tools.Operators {
    class EqualsEquals : SimpleOperator {
        public EqualsEquals(IOperator left, IOperator right, int row, int col) : base(left, right, "=", row, col) { }
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            return new Values.BooleanLiteral(leftResult.Equals(rightResult)); // if left is null
        }
    }
}