namespace Tools.Operators {
    class SimpleVariableOperator : SimpleOperator {
        protected Stack Stack { get; }
        public SimpleVariableOperator(Stack stack, IOperator left, IOperator right, string name) : base(left, right, name) {
            this.Stack = stack;
        }
    }
}