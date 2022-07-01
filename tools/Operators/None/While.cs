namespace Tools.Operators {
    class While : SimpleOperator {
        public While(IOperator left, IOperator right, int row, int col) : base(left, right, "while", row, col) {
        } // left is condition, right is scope
        public override IValue Run(Stack Stack) {
            while(Left._Run(Stack).Boolean) {
                Stack.Push();
                IValue result = Right._Run(Stack);
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