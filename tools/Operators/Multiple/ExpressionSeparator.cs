namespace Tools.Operators {
    class ExpressionSeparator : Operator {
        private List<IOperator> Children { get; }
        public ExpressionSeparator(int row, int col, List<IOperator>? children = null) : base(row, col) {
            this.Children = (children == null) ? new List<IOperator>() : children;
        }
        public override IValue Run() {
            foreach(IOperator child in Children) {
                //Console.WriteLine("looking for result");
                IValue result = child._Run();
                //Console.WriteLine("got result");
                //Console.WriteLine(result.Default);
                if(result.Default == BasicTypes.RETURN) {
                    //Console.WriteLine($"caught {result.Default}");
                    return result.Var;
                }
            }
            //Console.WriteLine("finished");
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