namespace Tools.Operators {
    class Each : Operator {
        private string VarName { get; }
        private IOperator List { get; }
        private IOperator Body { get; }
        public Each(string varName, IOperator list, IOperator body, int row, int col) : base(row, col) {
            this.VarName = varName;
            this.List = list;
            this.Body = body;
        } // left is condition, right is scope
        public override IValue Run(Stack Stack) {
            Stack.Push();
            Values.Variable every = new Values.Variable(null);
            Stack.Head.Val.Add(VarName, every);
            Dictionary<string, Values.Variable> elements = List._Run(Stack).Object;
            foreach(KeyValuePair<string, Values.Variable> vari in elements) {
                Stack.Push();
                every.Var = vari.Value.Var;
                IValue result = Body._Run(Stack);
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
            return $"each({VarName} of {List.Print()})\n{Body.Print()}";
        }
    }
}