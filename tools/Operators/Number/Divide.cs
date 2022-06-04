namespace Tools.Operators {
    class Divide : SimpleVariableOperator {
        public Divide(Stack stack, IOperator left, IOperator right, int row, int col) : base(stack, left, right, "/", row, col) {}
        public override IValue Run() {
            IValue leftResult = Left._Run().Var;
            IValue rightResult = Right._Run().Var;
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number / rightResult.Number, Stack.Get("Number").Var);
            }
            throw new RadishException("Only numbers can be combined using division!");
        }
    }
}