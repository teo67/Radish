namespace Tools.Operators {
    class NullValue : IOperator {
        public NullValue() {}
        public IValue Run() {
            return new Values.NoneLiteral();
        }
        public string Print() {
            return "null";
        }
    }
}