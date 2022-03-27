namespace Tools.Operators {
    class Loop : VariableOperator {
        private ListSeparator Tags { get; }
        private IOperator Body { get; }
        public Loop(Stack stack, ListSeparator tags, IOperator body) : base(stack) {
            this.Tags = tags;
            this.Body = body;
        } // left is condition, right is scope
        public override IValue Run() {
            if(Tags.Children.Count != 3) {
                throw new Exception("Loops require three arguments!");
            }
            Stack.Push();
            Tags.Children[0].Run();
            while(Tags.Children[1].Run().Boolean) {
                Stack.Push();
                IValue result = Body.Run();
                Tags.Children[2].Run();
                Stack.Pop();
                if(result.Default == BasicTypes.RETURN) {
                    if(result.String == "out" || result.String == "cancel") {
                        Stack.Pop();
                        return result;
                    }
                }
            }
            Stack.Pop();
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return $"loop({Tags.Print()})\n{Body.Print()}";
        }
    }
}