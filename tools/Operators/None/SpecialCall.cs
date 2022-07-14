namespace Tools.Operators {
    class SpecialCall : SimpleOperator {
        private IOperator? ThisRef = null;
        public SpecialCall(IOperator left, IOperator right, int row, int col, IOperator? _this = null) : base(left, right, "call", row, col) {
            this.ThisRef = _this;
        }
        public override IValue Run(Stack Stack) {
            Dictionary<string, Values.Variable> ret = Right._Run(Stack).Object;
            List<IValue> passing = new List<IValue>();
            foreach(KeyValuePair<string, Values.Variable> pair in ret) {
                passing.Add(pair.Value.Var);
            }
            IValue left = Left._Run(Stack);
            IValue returned = left.Function(passing, ThisRef == null ? null : ThisRef._Run(Stack).Var);
            RadishException.Pop();
            return returned;
        }
        public override string Print() {
            return $"{Left.Print}({Right.Print()})";
        }
    }
}