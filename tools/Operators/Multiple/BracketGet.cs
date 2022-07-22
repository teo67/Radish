// only difference from get is that this requires an operator not a string
namespace Tools.Operators {
    class BracketGet : Operator {
        private IOperator Left { get; }
        private IOperator Name { get; }
        public BracketGet(IOperator left, IOperator name, int row, int col) : base(row, col) {
            this.Left = left;
            this.Name = name;
        }
        public override IValue Run(Stack Stack) {
            IValue result = Left._Run(Stack);
            IValue evald = result.Var;
            if(evald.Default == BasicTypes.STRING) {
                int index = (int)Name._Run(Stack).Number;
                return new Values.Character(result, index, "" + evald.String[index]);
            }
            string nameR = Name._Run(Stack).String;
            (Values.Variable?, IValue?) gotten = Values.ObjectLiteral.DeepGet(evald, nameR, evald);
            return new Values.PropertyHolder(gotten.Item1, nameR, evald, gotten.Item2, gotten.Item1 == null ? ProtectionLevels.PUBLIC : gotten.Item1.ProtectionLevel);
        }

        public override string Print() {
            return $"({Left.Print()}[{Name.Print()}])";
        }
    }
}