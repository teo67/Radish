namespace Tools.Operators {
    class ArrayLength : Operator {
        private IOperator Input { get; }
        public ArrayLength(IOperator input) : base(-1, -1) {
            this.Input = input;
        }
        public override IValue Run(Stack Stack) {
            IValue result = Input._Run(Stack).Var;
            if(result.Default != BasicTypes.OBJECT) {
                throw new RadishException("Unable to take the length of a non-object value!", Row, Col);
            }
            return new Values.NumberLiteral(result.Object.Count);
        }
        public override string Print() {
            return $"{Input.Print()}.length";
        }
    }
}