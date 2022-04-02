namespace Tools.Operators {
    class Multiply : SimpleVariableOperator {
        public Multiply(Stack stack, IOperator left, IOperator right) : base(stack, left, right, "*") {}
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number * rightResult.Number, Stack.Get("Number").Var);
            }
            if(leftResult.Default == BasicTypes.STRING) {
                if(rightResult.Default == BasicTypes.NUMBER) {
                    string adding = "";
                    for(int i = 0; i < rightResult.Number; i++) {
                        adding += leftResult.String;
                    }
                    return new Values.StringLiteral(adding, Stack.Get("String").Var);
                }
                throw new Exception("Can only multiply a string by a number!");
            }
            throw new Exception("The multiply operator only applies to numbers and strings!");
        }
    }
}