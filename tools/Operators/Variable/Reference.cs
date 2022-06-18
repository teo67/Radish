namespace Tools.Operators {
    class Reference : VariableOperator {
        private string VarName { get; }
        private Librarian Librarian { get; }
        public Reference(Stack stack, string varName, int row, int col, Librarian librarian) : base(stack, row, col) {
            this.VarName = varName;
            this.Librarian = librarian;
        }
        public override IValue Run() {
            IValue? returned = Stack.SafeGet(VarName);
            if(returned != null) {
                return returned;
            }
            if(Librarian.Standard.Contains(VarName)) {
                return Librarian.Lookup(VarName, Row, Col);
            }
            throw new RadishException($"Variable {VarName} does not exist in the current scope or in the standard library!", Row, Col);
        }
        public override string Print() {
            return VarName;
        }
    }
}