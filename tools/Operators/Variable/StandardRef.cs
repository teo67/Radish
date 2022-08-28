namespace Tools.Operators {
    class StandardRef : Operator { // only difference is that it allows for special variables, and that standard files can't access other standard variables (instead, they should be imported manually)
        private string VarName { get; }
        private Librarian? Librarian { get; }
        private List<IOperator>? Args { get; }
        public StandardRef(string varName, int row, int col, Librarian? librarian, List<IOperator>? args) : base(row, col) {
            this.VarName = varName;
            this.Librarian = librarian;
            this.Args = args;
        }
        public override IValue Run(Stack Stack) {
            IValue? returned = Stack.SafeGet(VarName);
            if(returned != null) {
                return returned;
            }
            if(Librarian != null && Args != null) {
                IOperator res = Librarian.SpecialImport(VarName);
                Dictionary<string, Values.Variable> adding = new Dictionary<string, Values.Variable>();
                for(int i = 0; i < Args.Count; i++) {
                    adding.Add($"{i}", new Values.Variable(Args[i]._Run(Stack).Var));
                }
                Stack.Push(adding);
                IValue returning = res._Run(Stack);
                Stack.Pop();
                return returning;
            }
            throw new RadishException($"Variable {VarName} does not exist in the current scope!", Row, Col);
        }
        public override string Print() {
            return $"(stdlib {VarName})";
        }
    }
}