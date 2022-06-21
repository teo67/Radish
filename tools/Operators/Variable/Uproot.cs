namespace Tools.Operators {
    class Uproot : VariableOperator {
        private string VarName { get; }
        public Uproot(Stack stack, string varName, int row, int col) : base(stack, row, col) {
            this.VarName = varName;
        }
        public override IValue Run() {
            IValue? returning = null;
            for(int i = 0; i < Stack.Head.Val.Count; i++) {
                if(Stack.Head.Val[i].Name == VarName) {
                    returning = Stack.Head.Val[i];
                    Stack.Head.Val.RemoveAt(i);
                }
            }
            if(returning == null) {
                throw new RadishException($"No variable {VarName} found to prune!", Row, Col);
            }
            return returning;
        }
        public override string Print() {
            return $"(uproot {VarName})";
        }
    }
}