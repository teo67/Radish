namespace Tools.Operators {
    class Tangent  : SpecialOperator {
        public Tangent(Librarian librarian) : base(librarian) {
        }
        public override IValue Run(Stack Stack) {
            double res = GetArgument(0)._Run(Stack).Number;
            if(res % Math.PI == Math.PI / 2) {
                throw new RadishException("The tangent of any theta where theta % pi == pi / 2 is (+/-)infinity!", Row, Col);
            }
            return new Values.NumberLiteral(Math.Tan(res));
        }
        public override string Print() {
            return $"tan({GetArgument(0).Print()})";
        }
    }
}