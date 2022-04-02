namespace Tools.Operators {
    class Number : VariableOperator {
        private double Stored { get; }
        public Number(Stack stack, double stored) : base(stack) {
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