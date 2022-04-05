namespace Tools.Operators {
    class Push : SimpleOperator {
        public Push(IOperator left, IOperator right) : base(left, right, "push") {

        }
        public override IValue Run() {
            IValue left = Left.Run();
            left.Object.Add(new Values.Variable($"{left.Object.Count}", Right.Run().Clone())); // this is a sloppy method, optimized for runtime without validating whether the input is an array
            return left;
        }
    }
}