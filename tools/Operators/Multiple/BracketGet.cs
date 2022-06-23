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
            IValue result = Left._Run();
            IValue evald = result.Var;
            if(evald.Default == BasicTypes.STRING) {
                int index = (int)Name._Run().Number;
                return new Values.Character(result, index, "" + evald.String[index]);
            }
            string nameR = Name._Run().String;
            Values.Variable? gotten = Values.ObjectLiteral.DeepGet(evald, nameR, evald);
            return new Values.PropertyHolder(gotten, nameR, evald, gotten == null ? ProtectionLevels.PUBLIC : gotten.ProtectionLevel);
        }

        public override string Print() {
            return $"({Left.Print()}[{Name.Print()}])";
        }
    }
}