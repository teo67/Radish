namespace Tools.Operators {
    class CopyFunction : SpecialOperator {
        public CopyFunction(Librarian librarian) : base(librarian) {}
        public override IValue Run(Stack Stack) {
            IValue? ran = GetArgument(0)._Run(Stack).Var;
            return new Values.FunctionLiteral(ran.Function); // i am cheating lol
        }
        public override string Print() {
            return "anonymous function";
        }
    }
}