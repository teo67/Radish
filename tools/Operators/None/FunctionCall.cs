namespace Tools.Operators {
    class FunctionCall : SimpleOperator {
        private Stack Stack { get; }
        public FunctionCall(IOperator left, IOperator right, Stack stack, int row, int col) : base(left, right, "called with arguments", row, col) {
            this.Stack = stack;
        }
        public override IValue Run() {
            // IValue result = Left._Run();
            // Stack.Push(new List<Values.Variable>() {
            //     new Values.Variable("this", result)
            // });
            IValue returned = Left._Run().Function(Right._Run().Object);
            //Stack.Pop();
            return returned.Var;
        }
    }
}