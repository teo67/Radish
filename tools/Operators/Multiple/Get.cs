namespace Tools.Operators {
    class Get : Operator {
        private IOperator Left { get; }
        private string Name { get; }
        public Get(IOperator left, string name, int row, int col) : base(row, col) {
            this.Left = left;
            this.Name = name;
        }
        public override IValue Run() {
            IValue result = Left._Run().Var;
            //Console.WriteLine(result);
            Values.Variable? gotten = Values.ObjectLiteral.DeepGet(result, Name, result);
            //Console.WriteLine(gotten.Item1 == null ? "null" : gotten.Item1.Print());
            //Console.WriteLine(gotten.Item1);
            //Console.WriteLine($"property at {Row}, {Col}");
            return new Values.PropertyHolder(gotten, Name, result, gotten == null ? ProtectionLevels.PUBLIC : gotten.ProtectionLevel);

        }

        public override string Print() {
            return $"({Left.Print()}.{Name})";
        }
    }
}