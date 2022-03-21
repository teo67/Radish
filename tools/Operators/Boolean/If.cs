namespace Tools.Operators {
    class If : SimpleOperator {
        private Stack Stack { get; }
        public If(Stack stack, IOperator left, IOperator right) : base(left, right, "if") {
            this.Stack = stack;
        } // left is condition, right is scope
        public override IValue Run() {
            if(Left.Run().Boolean) {
                Stack.Push();
                Right.Run();
                Stack.Pop();
                return new Values.BooleanLiteral(true);
            }
            return new Values.BooleanLiteral(false);
        }
        public override string Print() {
            return $"if({Left.Print()})\n{Right.Print()}";
        }
    }
}