namespace Tools.Operators {
    class Log : Operator {
        private IOperator Input { get; }
        private IOperator Base { get; }
        public Log(IOperator input, IOperator _base) : base(-1, -1) {
            this.Input = input;
            this.Base = _base;
        }
        public override Tools.IValue Run(Stack Stack) {
            double input = Input._Run(Stack).Number;
            double _base = Base._Run(Stack).Number;
            if(input <= 0) {
                throw new RadishException($"Cannot take the logarithm of {input}, because it is a nonpositive number!", Row, Col);
            }
            if(_base <= 0) {
                throw new RadishException($"Cannot take a logarithm with base {_base}, because it is a nonpositive number!", Row, Col);
            }
            if(_base == 1) {
                throw new RadishException($"Cannot take the logarithm with base = 1, because 1 ** n == 1 for all n.", Row, Col);
            }
            return new Values.NumberLiteral(Math.Log(input, _base));
        }
        public override string Print() {
            return $"(log {Input.Print()} <- {Base.Print()})";
        }
    }
}