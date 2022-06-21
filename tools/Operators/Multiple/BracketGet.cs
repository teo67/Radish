// only difference from get is that this requires an operator not a string
namespace Tools.Operators {
    class BracketGet : Operator {
        private IOperator Left { get; }
        private IOperator Name { get; }
        public BracketGet(IOperator left, IOperator name, int row, int col) : base(row, col) {
            this.Left = left;
            this.Name = name;
        }
        public override IValue Run() {
            IValue result = Left._Run().Var;
            string nameR = Name._Run().String;
            Values.Variable? gotten = Values.ObjectLiteral.DeepGet(result, nameR, result);
            return new Values.PropertyHolder(gotten, nameR, result, gotten == null ? ProtectionLevels.PUBLIC : gotten.ProtectionLevel);
        }

        public override string Print() {
            return $"({Left.Print()}[{Name.Print()}])";
        }
    }
}