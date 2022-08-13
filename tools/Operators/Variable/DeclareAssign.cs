namespace Tools.Operators {
    class DeclareAssign : Operator {
        private string VarName { get; }
        private IOperator Value { get; }
        public DeclareAssign(IOperator value, string varName, int row, int col) : base(row, col) {
            this.VarName = varName;
            this.Value = value;
        }
        public override IValue Run(Stack Stack) {
            Values.Variable? returned = Stack.SafeGet(VarName);
            if(returned != null) {
                returned.Var = Value._Run(Stack).Var;
            } else {
                returned = new Values.Variable(Value._Run(Stack).Var);
                Stack.Head.Val.Add(VarName, returned);
            }
            return returned;
        }
        public override string Print() {
            return $"({Value.Print()})=>{VarName}";
        }
    }
}