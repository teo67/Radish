namespace Tools.Operators {
    class Arccosine : Operator {
        private IOperator Input { get; }
        public Arccosine(IOperator input) : base(-1, -1) {
            this.Input = input;
        }
        public override IValue Run(Stack Stack) {
            double res = Input._Run(Stack).Number;
            if(res < -1 || res > 1) {
                throw new RadishException($"Invalid argument {res} to arccos() - it should be within [-1, 1]! You could also be getting this error if you provided an invalid argument to arcsec().", Row, Col);
            }
            return new Values.NumberLiteral(Math.Acos(res));
        }
        public override string Print() {
            return $"acos({Input.Print()})";
        }
    }
}