namespace Tools.Values {
    class FunctionLiteral : EmptyLiteral {
        private Stack Stack { get; }
        private List<string> ArgNames { get; }
        public override IOperator FunctionBody { get; }
        public override List<Variable> Object { get; }
        public FunctionLiteral(Stack stack, List<string> argNames, IOperator body, IValue func) : base("function") {
            this.Stack = stack;
            this.ArgNames = argNames;
            this.FunctionBody = body;
            this.Object = new List<Variable>() {
                new Variable("base", func)
            };
        }
        public override BasicTypes Default {
            get {
                return BasicTypes.FUNCTION;
            }
        }
        public override bool Boolean {
            get {
                return true;
            }
        }
        public override IValue Function(List<IValue> args) {
            if(args.Count != ArgNames.Count) {
                throw new Exception("Invalid number of arguments!");
            }
            Stack.Push();
            for(int i = 0; i < args.Count; i++) {
                Stack.Head.Val.Add(new Variable(ArgNames[i], args[i]));
            }
            IValue result = FunctionBody.Run();
            Stack.Pop();
            if(result.Default != BasicTypes.RETURN) {
                return result; // this will be null
            }
            return result.Function(new List<IValue>());
        }
        public override IValue Clone() {
            return new FunctionLiteral(Stack, ArgNames, FunctionBody, ObjectLiteral.Get(this, "base"));
        }
        public override bool Equals(IValue other) {
            return FunctionBody == other.FunctionBody;
        }
    }
}