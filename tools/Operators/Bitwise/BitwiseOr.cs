namespace Tools.Operators {
    class BitwiseOr : BitwiseOperator {
        public BitwiseOr(Stack stack, IOperator left, IOperator right, int row, int col) : base(stack, left, right, "|", row, col) { }
        public override int GetResult(int left, int right) {
            return left | right;
        }
    }
}