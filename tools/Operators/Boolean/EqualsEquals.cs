namespace Tools.Operators {
    class EqualsEquals : SimpleVariableOperator {
        public EqualsEquals(Stack stack, IOperator left, IOperator right, int row, int col) : base(stack, left, right, "==", row, col) { }
        public override IValue Run() {
            IValue leftResult = Left._Run();
            IValue rightResult = Right._Run();
            return new Values.BooleanLiteral(leftResult.Equals(rightResult), Stack.Get("Boolean").Var); // if left is null
        }
    }
}