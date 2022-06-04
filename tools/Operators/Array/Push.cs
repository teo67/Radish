namespace Tools.Operators {
    class Push : SimpleOperator {
        public Push(IOperator left, IOperator right, int row, int col) : base(left, right, "push", row, col) {

        }
        public override IValue Run() {
            IValue left = Left._Run().Var;
            left.Object.Add(new Values.Variable($"{left.Object.Count}", Right._Run().Var)); // this is a sloppy method, optimized for runtime without validating whether the input is an array
            return left;
        }
    }
}