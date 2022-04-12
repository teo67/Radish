namespace Tools.Operators {
    class Assignment : SimpleOperator {
        public Assignment(IOperator left, IOperator right, int row, int col) : base(left, right, "p", row, col) {}
        public override IValue Run() {
            IValue result = Left._Run();
            IValue right = Right._Run();
            result.Var = right.Var;
            return result;
        }
    }
}