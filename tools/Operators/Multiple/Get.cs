namespace Tools.Operators {
    class Get : VariableOperator {
        private IOperator Left { get; }
        private string Name { get; }
        public Get(IOperator left, string name, Stack stack, int row, int col) : base(stack, row, col) {
            this.Left = left;
            this.Name = name;
        }
        public override IValue Run() {
            IValue result = Left._Run().Var;
            (IValue?, ProtectionLevels) gotten = Values.ObjectLiteral.DeepGet(result, Name, Stack, result);
            return new Values.PropertyHolder(gotten.Item1, Name, result, Stack, gotten.Item2);
        }

        public override string Print() {
            return $"({Left.Print()}.{Name})";
        }
    }
}