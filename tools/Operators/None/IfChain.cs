namespace Tools.Operators {
    class IfChain : Operator {
        private List<If> Children { get; }
        public IfChain(int row, int col, List<If>? children = null) : base(row, col) {
            this.Children = (children == null) ? new List<If>() : children;
        }
        public override IValue Run(Stack Stack) {
            foreach(If child in Children) {
                if(child.Check(Stack)) {
                    return child._Run(Stack);
                }
            }
            return new Values.NoneLiteral();
        }
        public void AddValue(If adding) {
            Children.Add(adding);
        }
        public override string Print() {
            string returning = "";
            foreach(IOperator child in Children) {
                returning += $"{child.Print()}\n";
            }
            return returning;
        }
    }
}