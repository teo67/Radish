namespace Tools.Values {
    class FunctionLiteral : EmptyLiteral {
        public static IValue? Proto { private get; set; }
        private Stack Stack { get; }
        private List<string> ArgNames { get; }
        private List<IOperator?> Defaults { get; }
        private bool Fill { get; }
        public override IOperator FunctionBody { get; }
        public override IValue? Base { get; }
        public override List<Variable> Object { get; } // setter is only used when defining a class
        public override IValue? IsSuper { get; set; }
        private string FileName { get; }
        public FunctionLiteral(Stack stack, List<string> argNames, List<IOperator?> defaults, bool fill, IOperator body, string fileName) : base("tool") {
            this.Stack = stack;
            this.ArgNames = argNames;
            this.Defaults = defaults;
            this.Fill = fill;
            this.FunctionBody = body;
            this.Base = Proto == null ? null : Proto.Var;
            this.Object = new List<Variable>();
            this.IsSuper = null;
            this.FileName = fileName;
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
        public override IValue Function(List<Variable> args, IValue? _this) {
            Stack.Push();
            if(IsSuper != null) {
                Stack.Head.Val.Add(new Variable("super", IsSuper)); 
            }
            IValue? saved = ObjectLiteral.CurrentPrivate;
            if(_this != null) {
                Stack.Head.Val.Add(new Variable("this", _this));
                ObjectLiteral.CurrentPrivate = _this;
            }
            for(int i = 0; i < ArgNames.Count - (Fill ? 1 : 0); i++) {
                IValue? host = null;
                if(args.Count > i) {
                    host = args[i].Var;
                } else {
                    IOperator? def = Defaults[i];
                    if(def != null) {
                        host = def._Run(Stack).Var;
                    } else {
                        throw new RadishException($"The following required argument was unsupplied: {ArgNames[i]}");
                    }
                }
                Stack.Head.Val.Add(new Variable(ArgNames[i], host));
            }
            if(Fill) {
                IValue? host = null;
                IOperator? def = Defaults[ArgNames.Count - 1];
                if(def != null && args.Count < ArgNames.Count) {
                    host = def._Run(Stack).Var;
                } else {
                    List<Values.Variable> arr = new List<Variable>();
                    for(int i = ArgNames.Count - 1; i < args.Count; i++) {
                        arr.Add(new Values.Variable($"{i - ArgNames.Count + 1}", args[i].Var));
                    }
                    host = new Values.ObjectLiteral(arr, useArrayProto: true);
                }
                Stack.Head.Val.Add(new Variable(ArgNames[ArgNames.Count - 1], host));
            }
            string previous = RadishException.FileName;
            RadishException.FileName = FileName;
            IValue result = FunctionBody._Run(Stack);
            RadishException.FileName = previous;
            Stack.Pop();
            ObjectLiteral.CurrentPrivate = saved;
            if(result.Default != BasicTypes.RETURN) {
                return result; // this will be null
            }
            return result.Function(new List<Variable>(), null);
        }
        public override bool Equals(IValue other) {
            return other.Default == BasicTypes.FUNCTION && FunctionBody == other.FunctionBody;
        }
        public override string Print() {
            return $"function";
        }
    }
}