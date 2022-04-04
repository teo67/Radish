namespace Tools.Operators {
    class ClassDefinition : VariableOperator {
        private IOperator Body { get; }
        public ClassDefinition(Stack stack, IOperator body) : base(stack) {
            this.Body = body;
        }
        public override IValue Run() {
            Stack.Push();
            IValue result = Body.Run();
            if(result.Default == BasicTypes.RETURN) {
                throw new Exception("Unable to output inside of an class definition!");
            }
            List<Values.Variable> popped = Stack.Pop().Val;
            IValue fun = new Values.FunctionLiteral(Stack, new List<string>(), new Operators.ExpressionSeparator(), Stack.Get("Function").Var);
            // empty constructor in case class has none
            int index = -1;
            for(int i = 0; i < popped.Count; i++) {
                if(popped[i].Name == "constructor") {
                    fun = popped[i].Var;
                    index = i;
                    break;
                }
            }
            if(index != -1) {
                popped.RemoveAt(index);
            }
            IValue proto = new Values.ObjectLiteral(popped, Stack.Get("Object").Var);
            fun.Object.Add(new Values.Variable("prototype", proto));
            return fun;
        }

        public override string Print() {
            return $"(class {Body.Print()})";
        }
    }
}