namespace Tools.Operators {
    class NullValue : Operator {
        public NullValue(int row, int col) : base(row, col) {}
        public override IValue Run(Stack Stack) {
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return "null";
        }
    }
}