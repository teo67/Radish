namespace Tools.Operators {
    class Objget : Operator {
        private IOperator Left { get; }
        private IOperator Name { get; }
        public Objget(IOperator left, IOperator name) : base(-1, -1) {
            this.Left = left;
            this.Name = name;
        }
        public override IValue Run(Stack Stack) {
            IValue result = Left._Run(Stack).Var;
            Values.Variable? gotten = Values.ObjectLiteral.DeepGet(result, Name._Run(Stack).String, new List<IValue>()).Item1;
            return gotten == null ? new Values.NoneLiteral() : gotten.Var;
        }

        public override string Print() {
            return $"(get {Left.Print()}.{Name})";
        }
    }
}