namespace Tools.Operators {
    class SimpleOperator : Operator {
        protected IOperator Left { get; }
        protected IOperator Right { get; }
        private string Name { get; }
        public SimpleOperator(IOperator left, IOperator right, string name, int row, int col) : base(row, col) {
            this.Left = left;
            this.Right = right;
            this.Name = name;
        }
        public override IValue Run() {
            throw new RadishException("Cannot run simple operator!");
        }
        public override string Print() {
            return $"({Left.Print()} {Name} {Right.Print()})";
        }
    }
}