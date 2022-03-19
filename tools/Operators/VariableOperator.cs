namespace Tools {
    class VariableOperator : IOperator {
        protected Stack Stack { get; }
        public VariableOperator(Stack stack) {
            this.Stack = stack;
        }
        public virtual IValue Run() {
            throw new Exception("Could not run a variable operator!");
        }
        public virtual string Print() {
            throw new Exception("Could not print a variable operator!");
        }
    }
}