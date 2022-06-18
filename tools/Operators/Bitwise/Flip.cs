namespace Tools.Operators {
    class Flip : VariableOperator {
        private IOperator Target { get; }
        public Flip(Stack stack, IOperator target, int row, int col) : base(stack, row, col) {
            this.Target = target;
        }
        public override IValue Run() {
            IValue result = Target._Run().Var;
            if(result.Default != BasicTypes.NUMBER) {
                throw new RadishException("Bitwise operations can only be performed on numbers!", Row, Col);
            }
            int asI = (int)result.Number;
            if(asI != result.Number) {
                throw new RadishException("Bitwise operations can only be performed on integer values!", Row, Col);
            }
            int final = ~asI;
            return new Values.NumberLiteral((double)final, Stack.Get("Number").Var);
        }
    }
}