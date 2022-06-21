namespace Tools.Operators {
    class RightShift : BitwiseOperator {
        public RightShift(IOperator left, IOperator right, int row, int col) : base(left, right, ">>", row, col) { }
        public override int GetResult(int left, int right) {
            return left >> right;
        }
    }
}