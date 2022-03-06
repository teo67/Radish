namespace Tools.Operators {
    class MoreThan : SimpleOperator {
        public MoreThan(IOperator left, IOperator right) : base(left, right, ">") { }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            return new Values.BooleanLiteral(leftResult.Number > rightResult.Number);
        }
    }
}