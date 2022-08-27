namespace Tools.Operators {
    class Arcsine  : SpecialOperator {
        public Arcsine(Librarian librarian) : base(librarian) {}
        public override IValue Run(Stack Stack) {
            double res = GetArgument(0)._Run(Stack).Number;
            if(res < -1 || res > 1) {
                throw new RadishException($"Invalid argument {res} to arcsin() - it should be within [-1, 1]! You could also be getting this error if you provided an invalid argument to arccsc().", Row, Col);
            }
            return new Values.NumberLiteral(Math.Asin(res));
        }
        public override string Print() {
            return $"asin({GetArgument(0).Print()})";
        }
    }
}