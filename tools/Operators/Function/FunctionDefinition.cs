namespace Tools.Operators {
    class FunctionDefinition : VariableOperator {
        private List<string> Args { get; }
        private List<IOperator?> Defaults { get; }
        private IOperator Body { get; }
        public FunctionDefinition(Stack stack, List<string> args, List<IOperator?> defaults, IOperator body, int row, int col) : base(stack, row, col) {
            this.Args = args;
            this.Defaults = defaults;
            this.Body = body;
        }
        public override IValue Run() {
            return new Values.FunctionLiteral(Stack, Args, Defaults, Body, Stack.Get("Function").Var);
        }
        public override string Print() {
            return "anonymous function";
        }
    }
}