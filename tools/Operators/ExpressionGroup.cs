namespace Tools.Operators {
    class ExpressionGroup : SimpleOperator<None, None, None> {
        public override IOperator<None> Left { get; }
        public override IOperator<None> Right { get; }
        public ExpressionGroup(IOperator<None> left, IOperator<None> right) : base(left, right) {
            this.Left = left;
            this.Right = right;
        }
        public override None Run() {
            Left.Run();
            Right.Run();
            return new None();
        }
    }
}