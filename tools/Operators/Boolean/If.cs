namespace Tools.Operators {
    class If : SimpleOperator {
        public If(IOperator left, IOperator right, int row, int col) : base(left, right, "if", row, col) {
        } // left is condition, right is scope
        public bool Check(Stack Stack) {
            return Left._Run(Stack).Boolean;
        }
        public override IValue Run(Stack Stack) {
            Stack.Push();
            IValue returned = Right._Run(Stack);
            Stack.Pop();
            return returned;
        }
        public override string Print() {
            return $"if({Left.Print()})\n{Right.Print()}";
        }
    }
}