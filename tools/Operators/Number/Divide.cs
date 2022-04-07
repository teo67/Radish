namespace Tools.Operators {
    class Divide : SimpleVariableOperator {
        public Divide(Stack stack, IOperator left, IOperator right) : base(stack, left, right, "/") {}
        public override IValue Run() {
            IValue leftResult = Left.Run().Var;
            IValue rightResult = Right.Run().Var;
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number / rightResult.Number, Stack.Get("Number").Var);
            }
            throw new Exception("The divide operator only applies to numbers!");
        }
    }
}