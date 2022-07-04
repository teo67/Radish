namespace Tools.Operators {
    class Divide : SimpleOperator {
        public Divide(IOperator left, IOperator right, int row, int col) : base(left, right, "/", row, col) {}
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            if(leftResult.Default == BasicTypes.NUMBER) {
                if(rightResult.Number == 0) {
                    throw new RadishException("Unable to divide by 0!");
                }
                return new Values.NumberLiteral(leftResult.Number / rightResult.Number);
            }
            throw new RadishException("Only numbers can be combined using division!");
        }
    }
}