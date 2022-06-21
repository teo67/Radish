namespace Tools.Operators {
    class And : SimpleOperator {
        public And(IOperator left, IOperator right, int row, int col) : base(left, right, "&&", row, col) { }
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            return new Values.BooleanLiteral(leftResult.Boolean && rightResult.Boolean);
        }
    }
}