namespace Tools.Operators {
    class Get : VariableOperator {
        private IOperator Left { get; }
        private string Name { get; }
        public Get(IOperator left, string name, Stack stack) : base(stack) {
            this.Left = left;
            this.Name = name;
        }
        public override IValue Run() {
            IValue result = Left.Run().Var;
            IValue? gotten = Values.ObjectLiteral.Get(result, Name, Stack, result);
            return new Values.PropertyHolder(gotten, Name, result, Stack);
        }

        public override string Print() {
            return $"({Left.Print()}.{Name})";
        }
    }
}