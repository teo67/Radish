namespace Tools.Operators {
    class Now : Operator {
        public Now() : base(-1, -1) {}
        public override IValue Run(Stack Stack) {
            long res = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            return new Values.NumberLiteral(res);
        }
    }
}