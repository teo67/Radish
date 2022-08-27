namespace Tools.Operators {
    class Cosine  : SpecialOperator {
        public Cosine(Librarian librarian) : base(librarian) {}
        public override IValue Run(Stack Stack) {
            return new Values.NumberLiteral(Math.Cos(GetArgument(0)._Run(Stack).Number));
        }
        public override string Print() {
            return $"cos({GetArgument(0).Print()})";
        }
    }
}