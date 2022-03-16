namespace Tools.Operators {
    class While : SimpleOperator {
        public While(IOperator left, IOperator right) : base(left, right, "while") {} // left is condition, right is scope
        public override IValue Run() {
            while(Left.Run().Boolean) {
                Right.Run();
            }
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return $"while({Left.Print()})\n{Right.Print()}";
        }
    }
}