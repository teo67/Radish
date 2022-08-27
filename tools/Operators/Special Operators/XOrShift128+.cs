namespace Tools.Operators {
    class XOrShift128Plus  : SpecialOperator {
        public XOrShift128Plus(Librarian librarian) : base(librarian) {
        }
        public override IValue Run(Stack Stack) {
            UInt64 s1 = (UInt64)GetArgument(0)._Run(Stack).Number;
            UInt64 s0 = (UInt64)GetArgument(1)._Run(Stack).Number;
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