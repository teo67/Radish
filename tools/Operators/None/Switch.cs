namespace Tools.Operators {
    class Switch : Operator {
        private IOperator Eval { get; }
        private List<IOperator> Cases { get; }
        private List<IOperator> Bodies { get; }
        private IOperator? Default { get; }
        public Switch(IOperator eval, List<IOperator> cases, List<IOperator> bodies, IOperator? def, int row, int col) : base(row, col) {
            this.Eval = eval;
            this.Cases = cases;
            this.Bodies = bodies;
            this.Default = def;
        }
        private IValue? Harv(IOperator runner, Stack Stack) {
            Stack.Push();
            IValue result = runner._Run(Stack);
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
            return null;        
        }
        public override IValue Run(Stack Stack) {
            IValue retrieved = Eval._Run(Stack).Var;
            bool started = false;
            Stack.Push();
            for(int i = 0; i < Cases.Count; i++) {
                if(!started) {
                    IValue res = Cases[i]._Run(Stack).Var;
                    if(retrieved.Equals(res)) {
                        started = true;
                    }
                }
                if(started) {
                    IValue? res1 = Harv(Bodies[i], Stack);
                    if(res1 != null) {
                        return res1;
                    }
                }
            }
            if(Default != null) {
                IValue? res = Harv(Default, Stack);
                if(res != null) {
                    return res;
                }
            }
            Stack.Pop();
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return $"switch({Eval.Print()}) {{...}}";
        }
    }
}