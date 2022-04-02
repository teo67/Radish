namespace Tools.Operators {
    class Invert : VariableOperator {
        private IOperator Target { get; }
        public Invert(Stack stack, IOperator target) : base(stack) {
            this.Target = target;
        }
        public override IValue Run() {
            return new Values.BooleanLiteral(!Target.Run().Boolean, Stack.Get("Boolean").Var);
        }
        public override string Print() {
            return $"(!{Target.Print()})";
        }
    }
}