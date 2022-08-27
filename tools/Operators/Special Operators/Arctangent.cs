namespace Tools.Operators {
    class Arctangent  : SpecialOperator {
        public Arctangent(Librarian librarian) : base(librarian) {}
        public override IValue Run(Stack Stack) {
            double res = GetArgument(0)._Run(Stack).Number;
            return new Values.NumberLiteral(Math.Atan(res));
        }
        public override string Print() {
            return $"atan({GetArgument(0).Print()})";
        }
    }
}