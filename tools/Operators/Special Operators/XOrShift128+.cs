namespace Tools.Operators {
    class XOrShift128Plus : Operator {
        private IOperator S0 { get; }
        private IOperator S1 { get; }
        public XOrShift128Plus(IOperator s0, IOperator s1) : base(-1, -1) {
            S0 = s0;
            S1 = s1;
        }
        public override IValue Run(Stack Stack) {
            UInt64 s1 = (UInt64)S0._Run(Stack).Number;
            UInt64 s0 = (UInt64)S1._Run(Stack).Number;
         // returns what seed[1] should be set to
            s1 ^= s1 << 23;
            UInt64 res = s1 ^ s0 ^ (s1 >> 18) ^ (s0 >> 5);
            return new Values.NumberLiteral((double)res);
        }
        public override string Print() {
            return "RANDOM";
        }
    }
}