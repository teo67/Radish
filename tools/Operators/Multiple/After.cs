namespace Tools.Operators {
    class After : SimpleOperator {
        public After(IOperator left, IOperator right, int row, int col) : base(left, right, "after", row, col) {}
        public override IValue Run(Stack Stack) {
            IValue res = Left._Run(Stack);
            res.Base = Right._Run(Stack).Var;
            return res;
        }
    }
}