namespace Tools.Operators {
    class Round : SpecialOperator {
        public Round(Librarian librarian) : base(librarian) {}
        public override IValue Run(Stack Stack) {
            return new Values.NumberLiteral((int)Math.Round(GetArgument(0)._Run(Stack).Number));
        }
    }
}