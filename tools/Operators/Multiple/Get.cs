namespace Tools.Operators {
    class Get : SimpleOperator {
        private Stack Stack { get; }
        public Get(IOperator left, IOperator right, Stack stack) : base(left, right, "get") {
            this.Stack = stack;
        }
        public override IValue Run() {
            IValue result = Left.Run();
            IValue right = Right.Run();
            IValue gotten = Values.ObjectLiteral.Get(result, right.String);
            return new Values.PropertyHolder(gotten, right.String, result, Stack);
        }
    }
}