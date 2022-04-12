namespace Tools.Operators {
    class FunctionCall : SimpleOperator {
        private Stack Stack { get; }
        public FunctionCall(IOperator left, IOperator right, Stack stack, int row, int col) : base(left, right, "called with arguments", row, col) {
            this.Stack = stack;
        }
        public override IValue Run() {
            IValue returned = Left._Run().Function(Right._Run().Object);
            return returned.Var;
        }
        public override IValue OnError(RadishException error) {
            try {
                Left.OnError(error); // either returns or throws, if returns we throw a non-descriptive error
            } catch(RadishException e) {
                throw e.AppendToTop("(...)");
            }
            throw error.Append("@ function call", Row, Col);
        }
    }
}