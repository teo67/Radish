namespace Tools.Operators {
    class XNor : SimpleOperator {
        public XNor(IOperator left, IOperator right, int row, int col) : base(left, right, "xor", row, col) { }
        public override IValue Combine(IValue left, IValue right) {
            return new Values.BooleanLiteral(left.Boolean == right.Boolean);
        }
    }
}