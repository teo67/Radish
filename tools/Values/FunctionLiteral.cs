namespace Tools.Values {
    class FunctionLiteral : EmptyLiteral {
        private Stack Stack { get; }
        private List<string> ArgNames { get; }
        private IOperator Body { get; }
        public FunctionLiteral(Stack stack, List<string> argNames, IOperator body) : base("function") {
            this.Stack = stack;
            this.ArgNames = argNames;
            this.Body = body;
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
            IValue result = Body.Run();
            Stack.Pop();
            if(result.Default != BasicTypes.RETURN) {
                return result; // this will be null
            }
            return result.Function(new List<IValue>());
        }
    }
}