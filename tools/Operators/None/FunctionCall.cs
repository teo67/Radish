namespace Tools.Operators {
    class FunctionCall : Operator {
        private IOperator? ThisRef { get; }
        private IOperator Left { get; }
        private List<IOperator> Right { get; }
        public FunctionCall(IOperator left, List<IOperator> right, int row, int col, IOperator? _this = null) : base(row, col) {
            this.ThisRef = _this;
            this.Left = left;
            this.Right = right;
        }
        public override IValue Run(Stack Stack) {
            List<IValue> passing = new List<IValue>();
            foreach(IOperator op in Right) {
                passing.Add(op._Run(Stack));
            }
            RadishException.Append($"at {Left.Print()}()", Row, Col, "", false);
            IValue left = Left._Run(Stack);
            IValue returned = left.Function(passing, ThisRef == null ? null : ThisRef._Run(Stack).Var);
            RadishException.Pop();
            return returned;
        }
        public override string Print() {
            string ret = $"{Left.Print}(";
            foreach(IOperator op in Right) {
                ret += op.Print();
                ret += ", ";
            }
            return ret + ")";
        }
    }
}