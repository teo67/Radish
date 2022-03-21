namespace Tools.Operators {
    class ListSeparator : IOperator {
        public List<IOperator> Children { get; }
        public ListSeparator(List<IOperator>? children = null) {
            this.Children = (children == null) ? new List<IOperator>() : children;
        }
        public IValue Run() {
            List<IValue> adding = new List<IValue>();
            foreach(IOperator child in Children) {
                adding.Add(child.Run());
            }
            return new Values.ArrayLiteral(adding);
        }
        public void AddValue(IOperator adding) {
            Children.Add(adding);
        }
        public string Print() {
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
        public List<string> ParseAsArgs() {
            List<string> returning = new List<string>();
            foreach(IOperator child in Children) {
                returning.Add(child.Print());
            }
            return returning;
        }
    }
}