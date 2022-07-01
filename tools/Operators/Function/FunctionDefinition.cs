namespace Tools.Operators {
    class FunctionDefinition : Operator {
        private List<string> Args { get; }
        private List<IOperator?> Defaults { get; }
        private bool Fill { get; }
        private IOperator Body { get; }
        private string FileName { get; }
        public FunctionDefinition(List<string> args, List<IOperator?> defaults, bool fill, IOperator body, int row, int col, string fileName) : base(row, col) {
            this.Args = args;
            this.Defaults = defaults;
            this.Body = body;
            this.FileName = fileName;
            this.Fill = fill;
        }
        public override IValue Run(Stack Stack) {
            return new Values.FunctionLiteral(new Stack(Stack.Head), Args, Defaults, Fill, Body, FileName); // create a new stack so that functions can use variables from their own layer
        }
        public override string Print() {
            return "anonymous function";
        }
    }
}