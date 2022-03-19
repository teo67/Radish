namespace Tools.Operators {
    class Reference : VariableOperator {
        private string VarName { get; }
        public Reference(Stack stack, string varName) : base(stack) {
            this.VarName = varName;
        }
        public override IValue Run() {
            return Stack.Get(VarName);
        }
        public override string Print() {
            return $"{VarName}";
        }
    }
}