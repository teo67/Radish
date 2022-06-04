namespace Tools.Operators {
    class String : VariableOperator {
        private string Stored { get; }
        public String(Stack stack, string stored, int row, int col) : base(stack, row, col) {
            this.Stored = stored;
        }
        public override IValue Run() {
            return new Values.StringLiteral(Stored, Stack.Get("String").Var);
        }
        public override string Print() {
            return $"\"{Stored}\"";
        }
    }
}