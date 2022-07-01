namespace Tools.Operators {
    class ObjectDefinition : Operator {
        private IOperator Body { get; }
        public ObjectDefinition(IOperator body, int row, int col) : base(row, col) {
            this.Body = body;
        }
        public override IValue Run(Stack Stack) {
            Stack.Push();
            IValue result = Body._Run(Stack);
            if(result.Default == BasicTypes.RETURN) {
                throw new RadishException("Unexpected return statement in an object literal definition!");
            }
            return new Values.ObjectLiteral(Stack.Pop().Val, useProto: true);
        }

        public override string Print() {
            return $"(object({Body.Print()}))";
        }
    }
}