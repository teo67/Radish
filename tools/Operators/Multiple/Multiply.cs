namespace Tools.Operators {
    class Multiply : SimpleVariableOperator {
        public Multiply(Stack stack, IOperator left, IOperator right, int row, int col) : base(stack, left, right, "*", row, col) {}
        public override IValue Run() {
            IValue leftResult = Left._Run().Var;
            IValue rightResult = Right._Run().Var;
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
                throw Error("Can only multiply a string by a number!");
            }
            throw Error("The multiply operator only applies to numbers and strings!");
        }
    }
}