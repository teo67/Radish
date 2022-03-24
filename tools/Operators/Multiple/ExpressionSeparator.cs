namespace Tools.Operators {
    class ExpressionSeparator : IOperator {
        private List<IOperator> Children { get; }
        public ExpressionSeparator(List<IOperator>? children = null) {
            this.Children = (children == null) ? new List<IOperator>() : children;
        }
        public IValue Run() {
            foreach(IOperator child in Children) {
                IValue result = child.Run();
                if(result.Default == BasicTypes.RETURN) {
                    return result;
                }
            }
            return new Values.NoneLiteral();
        }
        public void AddValue(IOperator adding) {
            Children.Add(adding);
        }
        public string Print() {
            string returning = "{\n";
            foreach(IOperator child in Children) {
                returning += $"{child.Print()}\n";
            }
            returning += "}";
            return returning;
        }
    }
}