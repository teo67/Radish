namespace Tools.Operators {
    class MoreThanOrEquals : SimpleVariableOperator {
        public MoreThanOrEquals(Stack stack, IOperator left, IOperator right) : base(stack, left, right, ">=") { }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            return new Values.BooleanLiteral(leftResult.Number >= rightResult.Number, Stack.Get("Boolean").Var);
        }
    }
}