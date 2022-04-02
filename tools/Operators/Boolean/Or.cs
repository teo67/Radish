namespace Tools.Operators {
    class Or : SimpleVariableOperator {
        public Or(Stack stack, IOperator left, IOperator right) : base(stack, left, right, "||") { }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            return new Values.BooleanLiteral(leftResult.Boolean || rightResult.Boolean, Stack.Get("Boolean").Var);
        }
    }
}