namespace Tools.Operators {
    class Number : VariableOperator {
        private double Stored { get; }
        public Number(Stack stack, double stored, int row, int col) : base(stack, row, col) {
            this.Stored = stored;
        }
        public override IValue Run() {
            return new Values.NumberLiteral(Stored, Stack.Get("Number").Var);
        }
        public override string Print() {
            return $"{Stored}";
        }
    }
}