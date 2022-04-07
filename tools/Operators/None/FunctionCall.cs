namespace Tools.Operators {
    class FunctionCall : SimpleOperator {
        private Stack Stack { get; }
        public FunctionCall(IOperator left, IOperator right, Stack stack) : base(left, right, "called with arguments") {
            this.Stack = stack;
        }
        public override IValue Run() {
            // IValue result = Left.Run();
            // Stack.Push(new List<Values.Variable>() {
            //     new Values.Variable("this", result)
            // });
            IValue returned = Left.Run().Function(Right.Run().Object);
            //Stack.Pop();
            return returned.Var;
        }
    }
}