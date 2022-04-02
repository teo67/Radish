namespace Tools.Operators {
    class Declaration : VariableOperator {
        private string VarName { get; }
        public Declaration(Stack stack, string varName) : base(stack) {
            this.VarName = varName;
        }
        public override IValue Run() {
            foreach(Values.Variable variable in Stack.Head.Val) {
                if(variable.Name == VarName) {
                    throw new Exception($"Could not redeclare variable {VarName}!");
                }
            }
            Values.Variable adding = new Values.Variable(VarName);
            Stack.Head.Val.Add(adding);
            return adding;
        }
        public override string Print() {
            return $"(declare variable {VarName})";
        }
    }
}








