namespace Tools.Operators {
    class ObjectDefinition : VariableOperator {
        private IOperator Body { get; }
        public ObjectDefinition(Stack stack, IOperator body, int row, int col) : base(stack, row, col) {
            this.Body = body;
        }
        public override IValue Run() {
            Stack.Push();
            IValue result = Body._Run();
            if(result.Default == BasicTypes.RETURN) {
                throw new RadishException("Unexpected return statement in an object literal definition!");
            }
            return new Values.ObjectLiteral(Stack.Pop().Val, Stack.Get("Object").Var);
        }

        public override string Print() {
            return $"(object({Body.Print()}))";
        }
    }
}