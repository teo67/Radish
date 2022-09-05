namespace Tools.Operators {
    class Flip : Operator {
        private IOperator Target { get; }
        public Flip(IOperator target, int row, int col) : base(row, col) {
            this.Target = target;
        }
        public override IValue Run(Stack Stack) {
            IValue result = Target._Run(Stack).Var;
            if(result.Default != BasicTypes.NUMBER && result.Default != BasicTypes.POLY) {
                throw new RadishException("Bitwise operations can only be performed on numbers!", Row, Col);
            }
            int asI = (int)result.Number;
            if(asI != result.Number) {
                throw new RadishException("Bitwise operations can only be performed on integer values!", Row, Col);
            }
            int final = ~asI;
            return new Values.NumberLiteral((double)final);
        }
        public override string Print() {
            return $"~{Target.Print()}";
        }
    }
}