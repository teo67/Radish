namespace Tools.Operators {
    class SimpleOperator : Operator {
        protected IOperator Left { get; }
        protected IOperator Right { get; }
        private string Name { get; }
        public bool IsAssigning { get; set; }
        public SimpleOperator(IOperator left, IOperator right, string name, int row, int col) : base(row, col) {
            this.Left = left;
            this.Right = right;
            this.Name = name;
            this.IsAssigning = false;
        }
        public virtual IValue Combine(IValue leftResult, IValue rightResult) {
            throw new RadishException("Cannot run simple operator!");
        }
        public override IValue Run(Stack Stack) {
            IValue left = Left._Run(Stack);
            IValue right = Right._Run(Stack);
            IValue result = Combine(left.Var, right.Var).Var;
            if(IsAssigning) {
                left.Var = result;
                return left;
            }
            return result;
        }
        public override string Print() {
            return $"({Left.Print()} {Name} {Right.Print()})";
        }
    }
}