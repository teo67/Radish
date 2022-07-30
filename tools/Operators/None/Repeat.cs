namespace Tools.Operators {
    class Repeat : SimpleOperator {
        public Repeat(IOperator left, IOperator right, int row, int col) : base(left, right, "repeat", row, col) {
        } // left is condition, right is scope
        public override IValue Run(Stack Stack) {
            double count = Left._Run(Stack).Number;
            for(int i = 0; i < count; i++) {
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
            return $"repeat({Left.Print()})\n{Right.Print()}";
        }
    }
}