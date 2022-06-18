namespace Tools.Operators {
    class And : SimpleVariableOperator {
        public And(Stack stack, IOperator left, IOperator right, int row, int col) : base(stack, left, right, "&&", row, col) { }
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            return new Values.BooleanLiteral(leftResult.Boolean && rightResult.Boolean, Stack.Get("Boolean").Var);
        }
    }
}