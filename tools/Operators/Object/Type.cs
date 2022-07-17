namespace Tools.Operators {
    class Type : Operator {
        private IOperator Typing { get; }
        public Type(IOperator typing, int row, int col) : base(row, col) {
            Typing = typing;
        }
        public override IValue Run(Stack Stack) {
            IValue? res = Typing._Run(Stack).Base;
            return res == null ? new Values.NoneLiteral() : res.Var;
        }
        public override string Print() {
            return $"type {Typing.Print()}";
        }
    }
}