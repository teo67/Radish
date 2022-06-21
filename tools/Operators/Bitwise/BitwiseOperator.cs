namespace Tools.Operators {
    class BitwiseOperator : SimpleOperator {
        public BitwiseOperator(IOperator left, IOperator right, string name, int row, int col) : base(left, right, name, row, col) {}
        public virtual int GetResult(int left, int right) {
            throw new RadishException("Cannot get the result of an empty bitwise operator!", Row, Col);
        }
        public override IValue Combine(IValue leftResult, IValue rightResult) {
            if(leftResult.Default != BasicTypes.NUMBER || rightResult.Default != BasicTypes.NUMBER) {
                throw new RadishException("Bitwise operations can only be performed on numbers!", Row, Col);
            }
            int leftI = (int)leftResult.Number;
            int rightI = (int)rightResult.Number;
            if(leftI != leftResult.Number || rightI != rightResult.Number) {
                throw new RadishException("Bitwise operations can only be performed on integer values!", Row, Col);
            }
            int result = GetResult(leftI, rightI);
            return new Values.NumberLiteral((double)result);
        }
    }
}