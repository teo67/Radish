namespace Tools.Operators {
    class MoreThan : SimpleVariableOperator {
        public MoreThan(Stack stack, IOperator left, IOperator right, int row, int col) : base(stack, left, right, ">", row, col) { }
        public override IValue Run() {
            IValue leftResult = Left._Run();
            IValue rightResult = Right._Run();
            return new Values.BooleanLiteral(leftResult.Number > rightResult.Number, Stack.Get("Boolean").Var);
        }
    }
}