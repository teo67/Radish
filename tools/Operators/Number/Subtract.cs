namespace Tools.Operators {
    class Subtract : SimpleOperator {
        public Subtract(IOperator left, IOperator right, int row, int col) : base(left, right, "-", row, col) {}
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number - rightResult.Number);
            }
            throw new RadishException("Only numbers can be combined using the subtract operator!");
        }
    }
}