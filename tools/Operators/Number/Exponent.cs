namespace Tools.Operators {
    class Exponent : SimpleOperator {
        public Exponent(IOperator left, IOperator right, int row, int col) : base(left, right, "/", row, col) {}
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            if(leftResult.Default == BasicTypes.NUMBER || leftResult.Default == BasicTypes.POLY) {
                if(leftResult.Number < 0 && rightResult.Number % 1 != 0) {
                    throw new RadishException($"{leftResult.Number} ** {rightResult.Number} would result in an imaginary number, which Radish does not support!");
                }
                return new Values.NumberLiteral(Math.Pow(leftResult.Number, rightResult.Number));
            }
            throw new RadishException("Only numbers can be combined using exponentiation!");
        }
    }
}