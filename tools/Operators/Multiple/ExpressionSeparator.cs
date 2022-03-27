namespace Tools.Operators {
    class ExpressionSeparator : IOperator {
        private List<IOperator> Children { get; }
        public ExpressionSeparator(List<IOperator>? children = null) {
            this.Children = (children == null) ? new List<IOperator>() : children;
        }
        public IValue Run() {
            foreach(IOperator child in Children) {
                //Console.WriteLine("looking for result");
                IValue result = child.Run();
                //Console.WriteLine("got result");
                //Console.WriteLine(result.Default);
                if(result.Default == BasicTypes.RETURN) {
                    //Console.WriteLine($"caught {result.Default}");
                    return result;
                }
            }
            //Console.WriteLine("finished");
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