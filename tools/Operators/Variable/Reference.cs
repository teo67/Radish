namespace Tools.Operators {
    class Reference : VariableOperator {
        private string VarName { get; }
        public Reference(Stack stack, string varName, int row, int col) : base(stack, row, col) {
            this.VarName = varName;
        }
        public override IValue Run() {
            return Stack.Get(VarName);
        }
        public override string Print() {
            return $"{VarName}";
        }
        public override IValue OnError(RadishException error) {
            throw error.Append($">> {VarName}", Row, Col);
        }
    }
}