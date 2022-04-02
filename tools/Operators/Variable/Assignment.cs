namespace Tools.Operators {
    class Assignment : SimpleOperator {
        public Assignment(IOperator left, IOperator right) : base(left, right, "=") {}
        public override IValue Run() {
            IValue result = Left.Run();
            IValue right = Right.Run();
            result.Var = right.Clone();
            return result;
        }
    }
}