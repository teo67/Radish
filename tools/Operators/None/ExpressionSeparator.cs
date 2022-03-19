namespace Tools.Operators {
    class ExpressionSeparator : VariableOperator, IOperator {
        private List<IOperator> Children { get; }
        public ExpressionSeparator(Stack variables, List<IOperator>? children = null): base(variables) {
            this.Children = (children == null) ? new List<IOperator>() : children;
        }
        public override IValue Run() {
            Stack.Push(new Dictionary<string, Values.Variable>());
            foreach(IOperator child in Children) {
                child.Run();
            }
            Stack.Pop();
            return new Values.NoneLiteral();
        }
        public void AddValue(IOperator adding) {
            Children.Add(adding);
        }
        public override string Print() {
            string returning = "{\n";
            foreach(IOperator child in Children) {
                returning += $"{child.Print()}\n";
            }
            returning += "}";
            return returning;
        }
    }
}