namespace Tools.Operators {
    class Lowercase : VariableOperator {
        private IOperator Stored { get; }
        public Lowercase(Stack stack, IOperator stored) : base(stack) {
            this.Stored = stored;
        }
        public override IValue Run() {
            return new Values.StringLiteral(Stored.Run().String.ToLower(), Stack.Get("String").Var);
        }
        public override string Print() {
            return $"(\"{Stored.Print()}\" to lower case)";
        }
    }
}