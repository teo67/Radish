namespace Tools.Operators {
    class IfChain : IOperator {
        private List<If> Children { get; }
        public IfChain(List<If>? children = null) {
            this.Children = (children == null) ? new List<If>() : children;
        }
        public IValue Run() {
            foreach(IOperator child in Children) {
                if(child.Run().Boolean) {
                    break;
                }
            }
            return new Values.NoneLiteral();
        }
        public void AddValue(If adding) {
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