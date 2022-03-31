namespace Tools.Operators {
    class PushTo : SimpleOperator {
        public PushTo(IOperator left, IOperator right) : base(left, right, "push to") {}
        public override IValue Run() {
            Left.Run().Array.Add(Right.Run());
            return new Values.NoneLiteral();
        }
    }
}