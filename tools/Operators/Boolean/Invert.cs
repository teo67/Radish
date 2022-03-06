namespace Tools.Operators {
    class Invert : IOperator {
        private IOperator Target { get; }
        public Invert(IOperator target) {
            this.Target = target;
        }
        public IValue Run() {
            return new Values.BooleanLiteral(!Target.Run().Boolean);
        }
        public string Print() {
            return $"(!{Target.Print()})";
        }
    }
}