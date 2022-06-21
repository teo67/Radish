namespace Tools.Operators {
    class Add : SimpleOperator {
        public Add(IOperator left, IOperator right, int row, int col) : base(left, right, "+", row, col) {}
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number + rightResult.Number);
            }
            if(leftResult.Default == BasicTypes.STRING) {
                return new Values.StringLiteral(leftResult.String + rightResult.String);
            }
            throw new RadishException("Only numbers and strings can be combined using addition!");
        }
    }
}