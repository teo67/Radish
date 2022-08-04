namespace Tools.Operators {
    class After : SimpleOperator {
        public After(IOperator left, IOperator right, int row, int col) : base(left, right, "after", row, col) {}
        public override IValue Run(Stack Stack) {
            IValue res = Left._Run(Stack);
            IValue other = Right._Run(Stack);
            res.Base = other.Var;
            return res;
        }
    }
}