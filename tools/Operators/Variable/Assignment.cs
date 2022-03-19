namespace Tools.Operators {
    class Assignment : SimpleOperator {
        public Assignment(IOperator left, IOperator right) : base(left, right, "=") {}
        public override IValue Run() {
            IValue result = Left.Run();
            result.Var.Carrying = Right.Run();
            return result;
        }
    }
}