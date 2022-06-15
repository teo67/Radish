namespace Tools.Operators {
    class Reference : VariableOperator {
        private string VarName { get; }
        public Reference(Stack stack, string varName, int row, int col) : base(stack, row, col) {
            this.VarName = varName;
        }
        public override IValue Run() {
            if(Librarian.Standard.Contains(VarName)) {
                return Librarian.Lookup(VarName, Row, Col);
            }
            return Stack.Get(VarName);
        }
        public override string Print() {
            return VarName;
        }
    }
}