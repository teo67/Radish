namespace Tools.Operators {
    class FunctionDefinition : VariableOperator {
        private List<string> Args { get; }
        private List<IOperator?> Defaults { get; }
        private IOperator Body { get; }
        private string FileName { get; }
        public FunctionDefinition(Stack stack, List<string> args, List<IOperator?> defaults, IOperator body, int row, int col, string fileName) : base(stack, row, col) {
            this.Args = args;
            this.Defaults = defaults;
            this.Body = body;
            this.FileName = fileName;
        }
        public override IValue Run() {
            return new Values.FunctionLiteral(Stack, Args, Defaults, Body, FileName);
        }
        public override string Print() {
            return "anonymous function";
        }
    }
}