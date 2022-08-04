namespace Tools.Operators {
    class ObjectDefinition : Operator {
        private IOperator Body { get; }
        private IOperator? Inh { get; }
        public ObjectDefinition(IOperator body, IOperator? inh, int row, int col) : base(row, col) {
            this.Body = body;
            this.Inh = inh;
        }
        public override IValue Run(Stack Stack) {
            Stack.Push();
            IValue result = Body._Run(Stack);
            if(result.Default == BasicTypes.RETURN) {
                throw new RadishException("Unexpected return statement in an object literal definition!");
            }
            Dictionary<string, Values.Variable> popped = Stack.Pop().Val;
            if(Inh == null) {
                return new Values.ObjectLiteral(popped, useProto: true);
            }
            return new Values.ObjectLiteral(popped, Inh._Run(Stack).Var);
        }

        public override string Print() {
            return $"(object {Body.Print()})";
        }
    }
}