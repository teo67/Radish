namespace Tools.Operators {
    class NullValue : Operator {
        public NullValue(int row, int col) : base(row, col) {}
        public override IValue Run() {
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return "null";
        }
    }
}