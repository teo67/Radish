namespace Tools.Operators {
    class StandardRef : Operator { // only difference is that it allows for special variables, and that standard files can't access other standard variables (instead, they should be imported manually)
        private string VarName { get; }
        private Librarian Librarian { get; }
        public StandardRef(string varName, int row, int col, Librarian librarian) : base(row, col) {
            this.VarName = varName;
            this.Librarian = librarian;
        }
        public override IValue Run(Stack Stack) {
            IValue? returned = Stack.SafeGet(VarName);
            if(returned != null) {
                return returned;
            }
            return Librarian.SpecialImport(VarName);
        }
        public override string Print() {
            return $"(stdlib {VarName})";
        }
    }
}