namespace Tools.Operators {
    class FunctionCall : SimpleOperator {
        private IOperator? ThisRef { get; }
        public FunctionCall(IOperator left, IOperator right, int row, int col, IOperator? _this = null) : base(left, right, "called with arguments", row, col) {
            this.ThisRef = _this;
        }
        public override IValue Run(Stack Stack) {
            List<Values.Variable> args = Right._Run(Stack).Object;
            RadishException.Append($"at {Left.Print()}()", Row, Col, "", false);
            IValue left = Left._Run(Stack);
            IValue returned = left.Function(args, ThisRef == null ? null : ThisRef._Run(Stack).Var);
            RadishException.Pop();
            return returned;
        }
    }
}