namespace Tools.Operators {
    class All : Operator {
        public All(int row, int col) : base(row, col) {}
        public override IValue Run(Stack Stack) {
            return new Values.ObjectLiteral(Stack.Head.Val, useProto: true);
        }
        public override string Print() {
            return "all";
        }
    }
}