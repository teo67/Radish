namespace Tools.Operators {
    class EqualsEquals : SimpleVariableOperator {
        public EqualsEquals(Stack stack, IOperator left, IOperator right) : base(stack, left, right, "==") { }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            return new Values.BooleanLiteral(leftResult.Equals(rightResult), Stack.Get("Boolean").Var); // if left is null
        }
    }
}