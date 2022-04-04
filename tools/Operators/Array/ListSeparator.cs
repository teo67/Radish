namespace Tools.Operators {
    class ListSeparator : VariableOperator {
        public List<IOperator> Children { get; }
        public ListSeparator(Stack stack, List<IOperator>? children = null) : base(stack) {
            this.Children = (children == null) ? new List<IOperator>() : children;
        }
        public override IValue Run() {
            List<Values.Variable> adding = new List<Values.Variable>();
            for(int i = 0; i < Children.Count; i++) {
                adding.Add(new Values.Variable($"{i}", Children[i].Run().Clone()));
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
                throw new Exception("List is missing an element!");
            }
            return Children[0];
        }
    }
}