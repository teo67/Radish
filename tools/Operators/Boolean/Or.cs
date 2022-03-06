namespace Tools.Operators {
    class Or : SimpleOperator {
        public Or(IOperator left, IOperator right) : base(left, right, "||") { }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            return new Values.BooleanLiteral(leftResult.Boolean || rightResult.Boolean);
        }
    }
}