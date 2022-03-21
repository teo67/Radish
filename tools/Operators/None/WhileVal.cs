namespace Tools.Operators {
    class WhileVal : VariableOperator {
        private ListSeparator Tags { get; }
        private IOperator Body { get; }
        public WhileVal(Stack stack, ListSeparator tags, IOperator body) : base(stack) {
            this.Tags = tags;
            this.Body = body;
        } // left is condition, right is scope
        public override IValue Run() {
            if(Tags.Children.Count != 3) {
                throw new Exception("Whileval loops require three arguments!");
            }
            Stack.Push();
            Tags.Children[0].Run();
            while(Tags.Children[1].Run().Boolean) {
                Stack.Push();
                Body.Run();
                Tags.Children[2].Run();
                Stack.Pop();
            }
            Stack.Pop();
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return $"whileval({Tags.Print()})\n{Body.Print()}";
        }
    }
}