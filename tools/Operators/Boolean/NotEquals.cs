namespace Tools.Operators {
    class NotEquals : SimpleVariableOperator {
        public NotEquals(Stack stack, IOperator left, IOperator right, int row, int col) : base(stack, left, right, "!=", row, col) {}
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            return new Values.BooleanLiteral(!leftResult.Equals(rightResult), Stack.Get("Boolean").Var); // if left is null
        }
    }
}