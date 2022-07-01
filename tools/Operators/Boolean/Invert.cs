namespace Tools.Operators {
    class Invert : Operator {
        private IOperator Target { get; }
        public Invert(IOperator target, int row, int col) : base(row, col) {
            this.Target = target;
        }
        public override IValue Run(Stack Stack) {
            return new Values.BooleanLiteral(!Target._Run(Stack).Boolean);
        }
        public override string Print() {
            return $"(!{Target.Print()})";
        }
    }
}