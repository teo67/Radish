namespace Tools.Operators {
    class FromJSON  : SpecialOperator {
        public FromJSON(Librarian librarian) : base(librarian) {
        }
        public override IValue Run(Stack Stack) {
            string retr = GetArgument(0)._Run(Stack).String;
            JSONParser jsp = new JSONParser(retr);
            return jsp.Parse();
        }
        public override string Print() {
            return $"({GetArgument(0).Print()} from json)";
        }
    }
}