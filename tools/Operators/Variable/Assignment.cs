namespace Tools.Operators {
    class Assignment : SimpleOperator {
        public Assignment(IOperator left, IOperator right, int row, int col) : base(left, right, "p", row, col) {}
        public override IValue Run(Stack Stack) {
            IValue result = Left._Run(Stack);
            IValue right = Right._Run(Stack);
            result.Var = right.Var;
            return result;
        }
    }
}