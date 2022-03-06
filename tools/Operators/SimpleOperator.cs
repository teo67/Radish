namespace Tools.Operators {
    class SimpleOperator : IOperator {
        protected IOperator Left { get; }
        protected IOperator Right { get; }
        private string Name { get; }
        public SimpleOperator(IOperator left, IOperator right, string name) {
            this.Left = left;
            this.Right = right;
            this.Name = name;
        }
        public virtual IValue Run() {
            throw new Exception("Cannot run simple operator!");
        }
        public virtual string Print() {
            return $"({Left.Print()} {Name} {Right.Print()})";
        }
    }
}