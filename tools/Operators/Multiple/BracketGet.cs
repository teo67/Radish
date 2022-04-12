// only difference from get is that this requires an operator not a string
namespace Tools.Operators {
    class BracketGet : VariableOperator {
        private IOperator Left { get; }
        private IOperator Name { get; }
        public BracketGet(IOperator left, IOperator name, Stack stack, int row, int col) : base(stack, row, col) {
            this.Left = left;
            this.Name = name;
        }
        public override IValue Run() {
            IValue result = Left._Run().Var;
            string nameR = Name._Run().String;
            (IValue?, ProtectionLevels) gotten = Values.ObjectLiteral.DeepGet(result, nameR, Stack, result);
            return new Values.PropertyHolder(gotten.Item1, nameR, result, Stack, gotten.Item2);
        }

        public override string Print() {
            return $"({Left.Print()}[{Name.Print()}])";
        }
    }
}