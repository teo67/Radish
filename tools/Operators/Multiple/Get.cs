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
            if(gotten.Default == BasicTypes.FUNCTION) {
                return new Values.ThisFunction(result, gotten, Stack);
            }
            return gotten;
        }
    }
}