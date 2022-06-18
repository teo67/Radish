namespace Tools.Operators {
    class LeftShift : BitwiseOperator {
        public LeftShift(Stack stack, IOperator left, IOperator right, int row, int col) : base(stack, left, right, "<<", row, col) { }
        public override int GetResult(int left, int right) {
            return left << right;
        }
    }
}