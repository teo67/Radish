namespace Tools.Operators {
    class StringLength : Operator {
        private IOperator Input { get; }
        public StringLength(IOperator input) : base(-1, -1) {
            this.Input = input;
        }
        public override IValue Run(Stack Stack) {
            IValue result = Input._Run(Stack).Var;
            if(result.Default != BasicTypes.STRING) {
                throw new RadishException("Unable to take the length of a non-string value!", Row, Col);
            }
            return new Values.NumberLiteral(result.String.Length);
        }
        public override string Print() {
            return $"{Input.Print()}.length";
        }
    }
}