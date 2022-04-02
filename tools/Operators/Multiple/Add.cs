namespace Tools.Operators {
    class Add : SimpleVariableOperator {
        public Add(Stack stack, IOperator left, IOperator right) : base(stack, left, right, "+") {}
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number + rightResult.Number, Stack.Get("Number").Var);
            }
            if(leftResult.Default == BasicTypes.STRING) {
                return new Values.StringLiteral(leftResult.String + rightResult.String, Stack.Get("String").Var);
            }
            throw new Exception("The add operator only applies to numbers and strings!");
        }
    }
}