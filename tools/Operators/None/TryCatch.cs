namespace Tools.Operators {
    class TryCatch : SimpleVariableOperator {
        public TryCatch(IOperator left, IOperator right, Stack stack, int row, int col) : base(stack, left, right, "-> catch ->", row, col) {}
        public override IValue Run() {
            StackNode top = Stack.Head;
            int len = RadishException.Entries.Count;
            try {
                Stack.Push();
                IValue returned = Left._Run(); // breakpoint will be here
                Stack.Pop();
                return returned;
            } catch {
                Stack.Head = top; // revert stack in case weird things happened
                Stack.Push(new List<Values.Variable>() {
                    new Values.Variable("error", new Values.StringLiteral(RadishException.Entries.Pop().RMessage, Stack.Get("String")))
                });
                while(RadishException.Entries.Count > len) {
                    RadishException.Entries.Pop();
                }
                IValue returned = Right._Run();
                Stack.Pop();
                return returned;
            }
        }
    }
}