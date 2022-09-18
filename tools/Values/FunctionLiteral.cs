namespace Tools.Values {
    class FunctionLiteral : EmptyLiteral {
        public static IValue? Proto { private get; set; }
        public override IValue? Base { get; set; }
        public override Dictionary<string, Variable> Object { get; } // setter is only used when defining a class
        public override Func<List<IValue>, IValue> Function { get; }
        public FunctionLiteral(Stack stack, List<string> argNames, List<IOperator?> defaults, bool fill, IOperator body, string fileName) : base("tool") {
            this.Object = new Dictionary<string, Variable>();
            this.Function = (List<IValue> args) => {
                stack.Push();
                if(this.Base != null) {
                    stack.Head.Val.Add("super", new Variable(new FunctionLiteral((List<IValue> _li) => this.Base.Function(_li), this.Base.Base, this.Base.Object)));
                }
                IValue? ct = ObjectLiteral.CurrentThis;
                if(ct != null) {
                    stack.Head.Val.Add("this", new Variable(ct));
                }
                for(int i = 0; i < argNames.Count - (fill ? 1 : 0); i++) {
                    IValue? host = null;
                    if(args.Count > i) {
                        host = args[i].Var;
                    } else {
                        IOperator? def = defaults[i];
                        if(def != null) {
                            host = def._Run(stack).Var;
                        } else {
                            throw new RadishException($"The following required argument was unsupplied: {argNames[i]}");
                        }
                    }
                    stack.Head.Val.Add(argNames[i], new Variable(host));
                }
                if(fill) {
                    IValue? host = null;
                    IOperator? def = defaults[argNames.Count - 1];
                    if(def != null && args.Count < argNames.Count) {
                        host = def._Run(stack).Var;
                    } else {
                        Dictionary<string, Values.Variable> arr = new Dictionary<string, Variable>();
                        for(int i = argNames.Count - 1; i < args.Count; i++) {
                            arr.Add($"{i - argNames.Count + 1}", new Variable(args[i].Var));
                        }
                        host = new Values.ObjectLiteral(arr, useArrayProto: true);
                    }
                    stack.Head.Val.Add(argNames[argNames.Count - 1], new Variable(host));
                }
                string previous = RadishException.FileName;
                RadishException.FileName = fileName;
                IValue result = body._Run(stack);
                RadishException.FileName = previous;
                stack.Pop();
                if(result.Default != BasicTypes.RETURN) {
                    return result; // this will be null
                }
                return result.Function(new List<IValue>());
            };
            this.Base = Proto == null ? null : Proto.Var;
        }
        public FunctionLiteral(Func<List<IValue>, IValue> function, IValue? _base = null, Dictionary<string, Variable>? obj = null) : base("tool") {
            this.Base = _base == null ? (Proto == null ? null : Proto.Var) : _base;
            this.Object = obj == null ? new Dictionary<string, Variable>() : obj;
            //this.Function = function;
            this.Function = function;
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
        public override bool Equals(IValue other) {
            return other.Default == BasicTypes.FUNCTION && Function == other.Function;
        }
        public override string Print() {
            return $"function";
        }
    }
}