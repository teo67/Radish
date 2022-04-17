namespace Tools.Operators {
    class FunctionDefinition : VariableOperator {
        private List<string> Args { get; }
        private IOperator Body { get; }
        public FunctionDefinition(Stack stack, List<string> args, IOperator body, int row, int col) : base(stack, row, col) {
            this.Args = args;
            this.Body = body;
        }
        public override IValue Run() {
            return new Values.FunctionLiteral(Stack, Args, Body, Stack.Get("Function").Var);
        }
        public override string Print() {
            return "anonymous function";
        }
    }
}