namespace Tools.Operators {
    class ListSeparator : VariableOperator {
        public List<IOperator> Children { get; }
        public ListSeparator(Stack stack, int row, int col, List<IOperator>? children = null) : base(stack, row, col) {
            this.Children = (children == null) ? new List<IOperator>() : children;
        }
        public override IValue Run() {
            List<Values.Variable> adding = new List<Values.Variable>();
            for(int i = 0; i < Children.Count; i++) {
                adding.Add(new Values.Variable($"{i}", Children[i]._Run().Var));
            }
            return new Values.ObjectLiteral(adding, Stack.Get("Array").Var);
        }
        public void AddValue(IOperator adding) {
            Children.Add(adding);
        }
        public override string Print() {
            string returning = "[";
            foreach(IOperator child in Children) {
                returning += $"{child.Print()}, ";
            }
            returning += "]";
            return returning;
        }
        public IOperator First() {
            if(Children.Count < 1) {
                throw new RadishException("Attempted to access element from empty list!");
            }
            return Children[0];
        }
    }
}