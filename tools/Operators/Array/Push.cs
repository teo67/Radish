namespace Tools.Operators {
    class Push : SimpleOperator {
        public Push(IOperator left, IOperator right) : base(left, right, "push") {

        }
        public override IValue Run() {
            IValue left = Left.Run();
            Left.Run().Array.Add(Right.Run());
            return left;
        }
    }
}