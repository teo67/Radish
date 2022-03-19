namespace Tools.Operators {
    class Declaration : VariableOperator {
        private string VarName { get; }
        public Declaration(Stack stack, string varName) : base(stack) {
            this.VarName = varName;
        }
        public override IValue Run() {
            if(Stack.Head.Val.ContainsKey(VarName)) {
                throw new Exception($"Could not redeclare variable {VarName}!");
            }
            Values.Variable adding = new Values.Variable();
            Stack.Head.Val[VarName] = adding;
            return adding;
        }
        public override string Print() {
            return $"(declare variable {VarName})";
        }
    }
}