namespace Tools.Operators {
    class FileExists : SpecialOperator {
        public FileExists(Librarian librarian) : base(librarian) {
        }
        public override IValue Run(Stack Stack) {
            string input = GetArgument(0)._Run(Stack).String;
            return new Values.BooleanLiteral(File.Exists(input));
        }
        public override string Print() {
            return $"(exist file {GetArgument(0).Print()})";
        }
    }
}