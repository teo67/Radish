namespace Tools.Operators {
    class StringLength  : SpecialOperator {
        public StringLength(Librarian librarian) : base(librarian) {
        }
        public override IValue Run(Stack Stack) {
            IValue result = GetArgument(0)._Run(Stack).Var;
            if(result.Default != BasicTypes.STRING) {
                throw new RadishException("Unable to take the length of a non-string value!", Row, Col);
            }
            return new Values.NumberLiteral(result.String.Length);
        }
        public override string Print() {
            return $"{GetArgument(0).Print()}.length";
        }
    }
}