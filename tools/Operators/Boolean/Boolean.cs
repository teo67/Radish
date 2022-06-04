namespace Tools.Operators {
    class Boolean : VariableOperator {
        private bool Stored { get; }
        public Boolean(bool stored, Stack stack, int row, int col) : base(stack, row, col) {
            this.Stored = stored;
        }
        public override IValue Run() {
            return new Values.BooleanLiteral(Stored, Stack.Get("Boolean").Var);
        }
        public override string Print() {
            return (Stored) ? "yes" : "no";
        }
    }
}