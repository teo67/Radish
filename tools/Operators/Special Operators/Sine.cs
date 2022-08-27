namespace Tools.Operators {
    class Sine  : SpecialOperator {
        public Sine(Librarian librarian) : base(librarian) {
        }
        public override IValue Run(Stack Stack) {
            return new Values.NumberLiteral(Math.Sin(GetArgument(0)._Run(Stack).Number));
        }
        public override string Print() {
            return $"sin({GetArgument(0).Print()})";
        }
    }
}