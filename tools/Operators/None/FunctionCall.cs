namespace Tools.Operators {
    class FunctionCall : SimpleOperator {
        public FunctionCall(IOperator left, IOperator right, int row, int col) : base(left, right, "called with arguments", row, col) {
        }
        public override IValue Run() {
            List<Values.Variable> args = Right._Run().Object;
            RadishException.Append($"at {Left.Print()}()", Row, Col, "", false);
            IValue left = Left._Run();
            IValue returned = left.Function(args, null);
            RadishException.Pop();
            return returned;
        }
    }
}