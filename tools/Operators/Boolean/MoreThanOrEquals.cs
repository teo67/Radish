namespace Tools.Operators {
    class MoreThanOrEquals : SimpleOperator {
        public MoreThanOrEquals(IOperator left, IOperator right) : base(left, right, ">=") { }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            return new Values.BooleanLiteral(leftResult.Number >= rightResult.Number);
        }
    }
}