namespace Tools.Operators {
    class Get : Operator {
        private IOperator Left { get; }
        private string Name { get; }
        public Get(IOperator left, string name, int row, int col) : base(row, col) {
            this.Left = left;
            this.Name = name;
        }
        public override IValue Run(Stack Stack) {
            IValue result = Left._Run(Stack).Var;
            //Console.WriteLine(result);
            (Values.Variable?, IValue?) gotten = Values.ObjectLiteral.DeepGet(result, Name, new List<IValue>());
            //Console.WriteLine(gotten.Item1 == null ? "null" : gotten.Item1.Print());
            //Console.WriteLine(gotten.Item1);
            //Console.WriteLine($"property at {Row}, {Col}");
            return new Values.PropertyHolder(gotten.Item1, Name, result, gotten.Item2, gotten.Item1 == null ? ProtectionLevels.PUBLIC : gotten.Item1.ProtectionLevel);

        }

        public override string Print() {
            return $"({Left.Print()}.{Name})";
        }
    }
}