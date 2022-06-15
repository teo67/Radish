namespace Tools.Operators {
    class StandardRef : VariableOperator { // only difference is that it allows for special variables, and that standard files can't access other standard variables (instead, they should be imported manually)
        private string VarName { get; }
        public StandardRef(Stack stack, string varName, int row, int col) : base(stack, row, col) {
            this.VarName = varName;
        }
        public override IValue Run() {
            IValue? returned = Librarian.SpecialImport(VarName);
            if(returned != null) {
                return returned;
            }
            return Stack.Get(VarName);
        }
        public override string Print() {
            return $"(stdlib {VarName})";
        }
    }
}