namespace Tools.Operators {
    class Uproot : Operator {
        private string VarName { get; }
        public Uproot(string varName, int row, int col) : base(row, col) {
            this.VarName = varName;
        }
        public override IValue Run(Stack Stack) {
            Values.Variable? returning = null;
            bool gotten = Stack.Head.Val.TryGetValue(VarName, out returning);
            if(!gotten || returning == null) {
                throw new RadishException($"No variable {VarName} found to prune!", Row, Col);
            }
            Stack.Head.Val.Remove(VarName);
            return returning;
        }
        public override string Print() {
            return $"(uproot {VarName})";
        }
    }
}