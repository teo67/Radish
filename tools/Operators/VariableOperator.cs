namespace Tools.Operators {
    class VariableOperator : Operator {
        protected Stack Stack { get; }
        public VariableOperator(Stack stack, int row, int col) : base(row, col) {
            this.Stack = stack;
        }
        public override IValue Run() {
            throw new RadishException("Could not run a variable operator!");
        }
        public override string Print() {
            throw new RadishException("Could not print a variable operator!");
        }
    }
}