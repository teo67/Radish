namespace Tools.Operators {
    class ArrayLength  : SpecialOperator {
        public ArrayLength(Librarian librarian) : base(librarian) {}
        public override IValue Run(Stack Stack) {
            IValue result = GetArgument(0)._Run(Stack).Var;
            return new Values.NumberLiteral(result.Object.Count);
        }
        public override string Print() {
            return $"{GetArgument(0).Print()}.length";
        }
    }
}