namespace Tools.Operators {
    class ObjectDefinition : VariableOperator {
        private IOperator Body { get; }
        public ObjectDefinition(Stack stack, IOperator body) : base(stack) {
            this.Body = body;
        }
        public override IValue Run() {
            Stack.Push();
            IValue result = Body.Run();
            if(result.Default == BasicTypes.RETURN) {
                throw new Exception("Unable to output inside of an object definition!");
            }
            return new Values.ObjectLiteral(Stack.Pop().Val, Stack.Get("Object").Var);
        }

        public override string Print() {
            return $"(object({Body.Print()}))";
        }
    }
}