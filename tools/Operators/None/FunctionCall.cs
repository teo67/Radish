namespace Tools.Operators {
    class FunctionCall : SimpleOperator {
        private Stack Stack { get; }
        public FunctionCall(IOperator left, IOperator right, Stack stack, int row, int col) : base(left, right, "called with arguments", row, col) {
            this.Stack = stack;
        }
        public override IValue Run() {
            List<Values.Variable> args = Right._Run().Object;
            RadishException.Append($"at {Left.Print()}()", Row, Col, false);
            IValue left = Left._Run();
            //Console.WriteLine(left.Print());
            //Stack.Print();
            IValue returned = left.Function(args);
            RadishException.Pop();
            return returned;
        }
    }
}