namespace Tools.Operators {
    class Log  : SpecialOperator {
        public Log(Librarian librarian) : base(librarian) {
        }
        public override Tools.IValue Run(Stack Stack) {
            double input = GetArgument(0)._Run(Stack).Number;
            double _base = GetArgument(1)._Run(Stack).Number;
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
            return $"(log {GetArgument(0).Print()} <- {GetArgument(1).Print()})";
        }
    }
}