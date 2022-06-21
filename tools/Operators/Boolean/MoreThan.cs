namespace Tools.Operators {
    class MoreThan : SimpleOperator {
        public MoreThan(IOperator left, IOperator right, int row, int col) : base(left, right, ">", row, col) { }
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            return new Values.BooleanLiteral(leftResult.Number > rightResult.Number);
        }
    }
}