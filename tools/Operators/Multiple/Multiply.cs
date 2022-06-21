namespace Tools.Operators {
    class Multiply : SimpleOperator {
        public Multiply(IOperator left, IOperator right, int row, int col) : base(left, right, "*", row, col) {}
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number * rightResult.Number);
            }
            if(leftResult.Default == BasicTypes.STRING) {
                if(rightResult.Default == BasicTypes.NUMBER) {
                    string adding = "";
                    for(int i = 0; i < rightResult.Number; i++) {
                        adding += leftResult.String;
                    }
                    return new Values.StringLiteral(adding);
                }
                throw new RadishException("Strings can only be multiplied by numbers!");
            }
            throw new RadishException("Only numbers and strings can be combined using multiplication!");
        }
    }
}