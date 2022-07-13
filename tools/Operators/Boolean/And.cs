namespace Tools.Operators {
    class And : SimpleOperator {
        public And(IOperator left, IOperator right, int row, int col) : base(left, right, "&&", row, col) { }
        public override IValue Run(Stack Stack) {
            IValue first = Left._Run(Stack).Var;
            if(!first.Boolean) {
                return first;
            }
            return Right._Run(Stack).Var;
        }
    }
}