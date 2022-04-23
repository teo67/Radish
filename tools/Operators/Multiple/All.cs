namespace Tools.Operators {
    class All : VariableOperator {
        public All(Stack stack, int row, int col) : base(stack, row, col) {}
        public override IValue Run() {
            return new Values.ObjectLiteral(Stack.Head.Val, Stack.Get("Object"));
        }
    }
}