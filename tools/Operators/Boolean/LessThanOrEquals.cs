namespace Tools.Operators {
    class LessThanOrEquals : SimpleOperator {
        public LessThanOrEquals(IOperator left, IOperator right) : base(left, right, "<=") { }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            return new Values.BooleanLiteral(leftResult.Number <= rightResult.Number);
        }
    }
}