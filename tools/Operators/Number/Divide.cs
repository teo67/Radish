namespace Tools.Operators {
    class Divide : SimpleVariableOperator {
        public Divide(Stack stack, IOperator left, IOperator right, int row, int col) : base(stack, left, right, "/", row, col) {}
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number / rightResult.Number, Stack.Get("Number").Var);
            }
            throw new RadishException("Only numbers can be combined using division!");
        }
    }
}