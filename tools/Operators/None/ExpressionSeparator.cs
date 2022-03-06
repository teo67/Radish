namespace Tools.Operators {
    class ExpressionSeparator : IOperator {
        private List<IOperator> Children { get; }
        public ExpressionSeparator(List<IOperator>? children = null) {
            this.Children = (children == null) ? new List<IOperator>() : children;
        }
        public IValue Run() {
            foreach(IOperator child in Children) {
                child.Run();
            }
            return new Values.NoneLiteral();
        }
        public void AddValue(IOperator adding) {
            Children.Add(adding);
        }
        public string Print() {
            string returning = "";
            foreach(IOperator child in Children) {
                returning += $"{child.Print()}\n";
            }
            return returning;
        }
    }
}