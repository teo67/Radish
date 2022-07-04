namespace Tools.Operators {
    class Arctangent : Operator {
        private IOperator Input { get; }
        public Arctangent(IOperator input) : base(-1, -1) {
            this.Input = input;
        }
        public override IValue Run(Stack Stack) {
            double res = Input._Run(Stack).Number;
            return new Values.NumberLiteral(Math.Atan(res));
        }
        public override string Print() {
            return $"atan({Input.Print()})";
        }
    }
}