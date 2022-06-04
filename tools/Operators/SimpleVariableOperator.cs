namespace Tools.Operators {
    class SimpleVariableOperator : SimpleOperator {
        protected Stack Stack { get; }
        public SimpleVariableOperator(Stack stack, IOperator left, IOperator right, string name, int row, int col) : base(left, right, name, row, col) {
            this.Stack = stack;
        }
    }
}