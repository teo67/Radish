namespace Tools.Operators {
    class While : SimpleOperator {
        private Stack Stack { get; }
        public While(Stack stack, IOperator left, IOperator right, int row, int col) : base(left, right, "while", row, col) {
            this.Stack = stack;
        } // left is condition, right is scope
        public override IValue Run() {
            while(Left._Run().Boolean) {
                Stack.Push();
                IValue result = Right._Run();
                Stack.Pop();
                if(result.Default == BasicTypes.RETURN) {
                    IValue asVar = result.Var;
                    if(asVar.String == "harvest" || asVar.String == "end") {
                        return asVar;
                    }
                    if(asVar.String == "cancel") {
                        return new Values.NoneLiteral();
                    }
                }
            }
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return $"while({Left.Print()})\n{Right.Print()}";
        }
    }
}