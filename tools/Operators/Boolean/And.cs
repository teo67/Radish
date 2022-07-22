namespace Tools.Operators {
    class And : SimpleOperator {
        public And(IOperator left, IOperator right, int row, int col) : base(left, right, "&&", row, col) { }
        public override IValue Run(Stack Stack) {
            IValue first = Left._Run(Stack);
            IValue firstvar = first.Var;
            IValue returning = firstvar;
            if(firstvar.Boolean) {
                returning = Right._Run(Stack).Var;
            }
            if(IsAssigning) {
                first.Var = returning;
                return first;
            }
            return returning;
        }
    }
}