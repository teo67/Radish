namespace Tools.Operators {
    class FunctionDefinition : VariableOperator {
        private ListSeparator Args { get; }
        private IOperator Body { get; }
        public FunctionDefinition(Stack stack, ListSeparator args, IOperator body) : base(stack) {
            this.Args = args;
            this.Body = body;
        }
        public override IValue Run() {
            return new Values.FunctionLiteral(Stack, Args.ParseAsArgs(), Body);
        }
    }
}