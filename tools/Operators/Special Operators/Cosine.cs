namespace Tools.Operators {
    class Cosine : Operator {
        private IOperator Input { get; }
        public Cosine(IOperator input) : base(-1, -1) {
            this.Input = input;
        }
        public override IValue Run(Stack Stack) {
            return new Values.NumberLiteral(Math.Cos(Input._Run(Stack).Number));
        }
        public override string Print() {
            return $"cos({Input.Print()})";
        }
    }
}