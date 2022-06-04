namespace Tools.Operators {
    class ReturnType : Operator {
        private IOperator? Stored { get; }
        private string Type { get; }
        public ReturnType(string type, int row, int col, IOperator? stored = null) : base(row, col) {
            string[] valid = { "harvest", "end", "cancel", "continue" };
            if(!valid.Contains(type)) {
                throw new RadishException($"{type} is not a valid return type!");
            }
            this.Type = type;
            this.Stored = stored;
        }
        public override IValue Run() {
            return new Values.ReturnType(Type, (Stored == null) ? new Values.NoneLiteral() : Stored._Run());
        }
        public override string Print() {
            return $"({Type} {((Stored == null) ? "none" : Stored.Print())}";
        }
    }
}