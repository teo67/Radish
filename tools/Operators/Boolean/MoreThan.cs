namespace Tools.Operators {
    class MoreThan : SimpleVariableOperator {
        public MoreThan(Stack stack, IOperator left, IOperator right, int row, int col) : base(stack, left, right, ">", row, col) { }
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            return new Values.BooleanLiteral(leftResult.Number > rightResult.Number, Stack.Get("Boolean").Var);
        }
    }
}