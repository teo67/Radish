namespace Tools.Operators {
    class Reference : Operator {
        private string VarName { get; }
        private Librarian Librarian { get; }
        public Reference(string varName, int row, int col, Librarian librarian) : base(row, col) {
            this.VarName = varName;
            this.Librarian = librarian;
        }
        public override IValue Run(Stack Stack) {
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