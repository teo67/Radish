namespace Tools.Operators {
    class Ternary : Operator {
        private IOperator Condition { get; }
        private IOperator First { get; }
        private IOperator Second { get; }
        public Ternary(IOperator condition, IOperator first, IOperator second, int row, int col) : base(row, col) {
            this.Condition = condition;
            this.First = first;
            this.Second = second;
        }
        public override IValue Run(Stack Stack) {
            bool res = Condition._Run(Stack).Boolean;
            return res ? First._Run(Stack) : Second._Run(Stack);
        }
        public override string Print() {
            return $"({Condition.Print()}) ? ({First.Print()}), ({Second.Print})";
        }
    }
}