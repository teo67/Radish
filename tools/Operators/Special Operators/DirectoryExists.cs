namespace Tools.Operators {
    class DirectoryExists  : SpecialOperator {
        public DirectoryExists(Librarian librarian) : base(librarian) {
        }
        public override IValue Run(Stack Stack) {
            string input = GetArgument(0)._Run(Stack).String;
            return new Values.BooleanLiteral(Directory.Exists(input));
        }
        public override string Print() {
            return $"(exist directory {GetArgument(0).Print()})";
        }
    }
}