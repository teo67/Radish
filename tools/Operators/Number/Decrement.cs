namespace Tools.Operators {
    class Decrement : VariableOperator {
        private IOperator Stored { get; }
        public Decrement(Stack stack, IOperator stored, int row, int col) : base(stack, row, col) {
            this.Stored = stored;
        }
        public override IValue Run() {
            IValue result = Stored._Run();
            result.Var = new Values.NumberLiteral(result.Number - 1, Stack.Get("Number").Var);
            return result;
        }
        public override string Print() {
            return $"({Stored.Print()}--)";
        }
    }
}