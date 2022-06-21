namespace Tools.Operators {
    class All : VariableOperator {
        public All(Stack stack, int row, int col) : base(stack, row, col) {}
        public override IValue Run() {
            return new Values.ObjectLiteral(Stack.Head.Val, useProto: true);
        }
        public override string Print() {
            return "all";
        }
    }
}