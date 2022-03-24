namespace Tools.Operators {
    class FunctionDefinition : VariableOperator {
        private ListSeparator Args { get; }
        private IOperator Body { get; }
        public FunctionDefinition(Stack stack, ListSeparator args, IOperator body) : base(stack) {
            this.Args = args;
            this.Body = body;
        }
        public override IValue Run() {
            List<IValue> argsAsValues = Args.Run().Array;
            List<string> args = new List<string>();
            foreach(IValue val in argsAsValues) {
                args.Add(val.String);
            }
            return new Values.FunctionLiteral(Stack, args, Body);
        }
        public override string Print() {
            return $"({Args.Print()}) {Body.Print()}";
        }
    }
}