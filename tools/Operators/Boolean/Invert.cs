namespace Tools.Operators {
    class Invert : VariableOperator {
        private IOperator Target { get; }
        public Invert(Stack stack, IOperator target, int row, int col) : base(stack, row, col) {
            this.Target = target;
        }
        public override IValue Run() {
            return new Values.BooleanLiteral(!Target._Run().Boolean, Stack.Get("Boolean").Var);
        }
        public override string Print() {
            return $"(!{Target.Print()})";
        }
    }
}