namespace Tools.Operators {
    class FunctionCall : SimpleOperator {
        public FunctionCall(IOperator left, IOperator right) : base(left, right, "called with arguments") {}
        public override IValue Run() {
            return Left.Run().Function(Right.Run().Array);
        }
    }
}