namespace Tools.Operators {
    class TryCatch : SimpleOperator {
        public TryCatch(IOperator left, IOperator right, int row, int col) : base(left, right, "-> catch ->", row, col) {}
        public override IValue Run(Stack Stack) {
            StackNode top = Stack.Head;
            int len = RadishException.Entries.Count;
            try {
                Stack.Push();
                IValue returned = Left._Run(Stack); // breakpoint will be here
                Stack.Pop();
                return returned;
            } catch {
                Stack.Head = top; // revert stack in case weird things happened
                Stack.Push(new List<Values.Variable>() {
                    new Values.Variable("error", new Values.StringLiteral(RadishException.Entries.Pop().RMessage))
                });
                while(RadishException.Entries.Count > len) {
                    RadishException.Entries.Pop();
                }
                IValue returned = Right._Run(Stack);
                Stack.Pop();
                return returned;
            }
        }
    }
}