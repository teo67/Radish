namespace Tools.Operators {
    class Loop : VariableOperator {
        private ListSeparator Tags { get; }
        private IOperator Body { get; }
        public Loop(Stack stack, ListSeparator tags, IOperator body, int row, int col) : base(stack, row, col) {
            this.Tags = tags;
            this.Body = body;
        } // left is condition, right is scope
        public override IValue Run() {
            if(Tags.Children.Count != 3) {
                throw new RadishException("For loops require three arguments: an initialize statement, a condition, and an iterative statement!");
            }
            Stack.Push();
            Tags.Children[0]._Run();
            while(Tags.Children[1]._Run().Boolean) {
                Stack.Push();
                IValue result = Body._Run();
                Tags.Children[2]._Run();
                Stack.Pop();
                if(result.Default == BasicTypes.RETURN) {
                    IValue asVar = result.Var;
                    if(asVar.String == "harvest" || asVar.String == "end") {
                        Stack.Pop();
                        return asVar;
                    }
                    if(asVar.String == "cancel") {
                        Stack.Pop();
                        return new Values.NoneLiteral();
                    }
                }
            }
            Stack.Pop();
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return $"for({Tags.Print()})\n{Body.Print()}";
        }
    }
}