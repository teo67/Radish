namespace Tools.Operators {
    class Sine : Operator {
        private IOperator Input { get; }
        public Sine(IOperator input) : base(-1, -1) {
            this.Input = input;
        }
        public override IValue Run(Stack Stack) {
            return new Values.NumberLiteral(Math.Sin(Input._Run(Stack).Number));
        }
        public override string Print() {
            return $"sin({Input.Print()})";
        }
    }
}