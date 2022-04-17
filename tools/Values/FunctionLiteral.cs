namespace Tools.Values {
    class FunctionLiteral : EmptyLiteral {
        private Stack Stack { get; }
        private List<string> ArgNames { get; }
        public override IOperator FunctionBody { get; }
        public override IValue? Base { get; }
        public override List<Variable> Object { get; } // setter is only used when defining a class
        public FunctionLiteral(Stack stack, List<string> argNames, IOperator body, IValue func) : base("tool") {
            this.Stack = stack;
            this.ArgNames = argNames;
            this.FunctionBody = body;
            this.Base = func;
            this.Object = new List<Variable>();
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
        public override IValue Function(List<Variable> args) {
            if(args.Count != ArgNames.Count) {
                //Console.WriteLine($"args {args.Count}, names {ArgNames.Count}");
                //Console.WriteLine(args[0].Name);
                //Console.WriteLine(args[1].Name);
                throw new RadishException($"Expecting {ArgNames.Count} arguments instead of {args.Count}!");
            }
            Stack.Push();
            for(int i = 0; i < args.Count; i++) {
                Stack.Head.Val.Add(new Variable(ArgNames[i], args[i].Var));
            }
            IValue result = FunctionBody._Run();
            Stack.Pop();
            if(result.Default != BasicTypes.RETURN) {
                return result; // this will be null
            }
            return result.Function(new List<Variable>());
        }
        public override bool Equals(IValue other) {
            return other.Default == BasicTypes.FUNCTION && FunctionBody == other.FunctionBody;
        }
    }
}