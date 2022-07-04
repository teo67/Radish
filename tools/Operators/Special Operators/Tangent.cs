namespace Tools.Operators {
    class Tangent : Operator {
        private IOperator Input { get; }
        public Tangent(IOperator input) : base(-1, -1) {
            this.Input = input;
        }
        public override IValue Run(Stack Stack) {
            double res = Input._Run(Stack).Number;
            if(res % Math.PI == Math.PI / 2) {
                throw new RadishException("The tangent of any theta where theta % pi == pi / 2 is (+/-)infinity!", Row, Col);
            }
            return new Values.NumberLiteral(Math.Tan(res));
        }
        public override string Print() {
            return $"tan({Input.Print()})";
        }
    }
}