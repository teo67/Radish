namespace Tools.Operators {
    class PolyAddString : SpecialOperator {
        public PolyAddString(Librarian librarian) : base(librarian) {

        }
        public override IValue Run(Stack Stack) {
            string a = GetArgument(0)._Run(Stack).String;
            string b = GetArgument(1)._Run(Stack).String;
            return new Values.StringLiteral(a + b);
        }
        public override string Print() {
            return $"({GetArgument(0).Print()} + {GetArgument(1).Print()})";
        }
    }
}