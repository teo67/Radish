namespace Tools.Operators {
    class If : SimpleOperator {
        private Stack Stack { get; }
        public If(Stack stack, IOperator left, IOperator right) : base(left, right, "if") {
            this.Stack = stack;
        } // left is condition, right is scope
        public bool Check() {
            return Left.Run().Boolean;
        }
        public override IValue Run() {
            Stack.Push();
            IValue returned = Right.Run();
            Stack.Pop();
            return returned;
        }
        public override string Print() {
            return $"if({Left.Print()})\n{Right.Print()}";
        }
    }
}