namespace Tools.Operators {
    class If : SimpleOperator {
        public If(IOperator left, IOperator right) : base(left, right, "if") {} // left is condition, right is scope
        public override IValue Run() {
            if(Left.Run().Boolean) {
                Right.Run();
                return new Values.BooleanLiteral(true);
            }
            return new Values.BooleanLiteral(false);
        }
        public override string Print() {
            return $"if({Left.Print()})\n{Right.Print()}";
        }
    }
}