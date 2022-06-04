namespace Tools.Operators {
    class If : SimpleOperator {
        private Stack Stack { get; }
        public If(Stack stack, IOperator left, IOperator right, int row, int col) : base(left, right, "if", row, col) {
            this.Stack = stack;
        } // left is condition, right is scope
        public bool Check() {
            return Left._Run().Boolean;
        }
        public override IValue Run() {
            Stack.Push();
            IValue returned = Right._Run();
            Stack.Pop();
            return returned;
        }
        public override string Print() {
            return $"if({Left.Print()})\n{Right.Print()}";
        }
    }
}