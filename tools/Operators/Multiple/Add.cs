namespace Tools.Operators {
    class Add : SimpleVariableOperator {
        public Add(Stack stack, IOperator left, IOperator right, int row, int col) : base(stack, left, right, "+", row, col) {}
        public override IValue Run() {
            IValue leftResult = Left._Run().Var;
            IValue rightResult = Right._Run().Var;
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number + rightResult.Number, Stack.Get("Number").Var);
            }
            if(leftResult.Default == BasicTypes.STRING) {
                return new Values.StringLiteral(leftResult.String + rightResult.String, Stack.Get("String").Var);
            }
            throw new RadishException("Only numbers and strings can be combined using addition!");
        }
    }
}