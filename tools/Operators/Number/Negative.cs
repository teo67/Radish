namespace Tools.Operators {
    class Negative : VariableOperator {
        private IOperator Stored { get; }
        public Negative(Stack stack, IOperator stored, int row, int col) : base(stack, row, col) {
            this.Stored = stored;
        }
        public override IValue Run() {
            IValue result = Stored._Run().Var;
            return new Values.NumberLiteral(result.Number * -1, Stack.Get("Number").Var);
        }
        public override string Print() {
            return $"(-{Stored.Print()})";
        }
    }
}