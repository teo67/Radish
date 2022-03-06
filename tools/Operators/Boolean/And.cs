namespace Tools.Operators {
    class And : SimpleOperator {
        public And(IOperator left, IOperator right) : base(left, right, "&&") { }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            return new Values.BooleanLiteral(leftResult.Boolean && rightResult.Boolean);
        }
    }
}