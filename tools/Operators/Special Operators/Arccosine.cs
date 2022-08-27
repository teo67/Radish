namespace Tools.Operators {
    class Arccosine  : SpecialOperator {
        public Arccosine(Librarian librarian) : base(librarian) {}
        public override IValue Run(Stack Stack) {
            double res = GetArgument(0)._Run(Stack).Number;
            if(res < -1 || res > 1) {
                throw new RadishException($"Invalid argument {res} to arccos() - it should be within [-1, 1]! You could also be getting this error if you provided an invalid argument to arcsec().", Row, Col);
            }
            return new Values.NumberLiteral(Math.Acos(res));
        }
        public override string Print() {
            return $"acos({GetArgument(0).Print()})";
        }
    }
}