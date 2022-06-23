namespace Tools.Values {
    class FunctionLiteral : EmptyLiteral {
        public static IValue? Proto { private get; set; }
        protected Stack Stack { get; }
        protected List<string> ArgNames { get; }
        protected List<IOperator?> Defaults { get; }
        public override IOperator FunctionBody { get; }
        public override IValue? Base { get; }
        public override List<Variable> Object { get; } // setter is only used when defining a class
        public override bool IsSuper { get; set; }
        private string FileName { get; }
        public FunctionLiteral(Stack stack, List<string> argNames, List<IOperator?> defaults, IOperator body, string fileName) : base("tool") {
            this.Stack = stack;
            this.ArgNames = argNames;
            this.Defaults = defaults;
            this.FunctionBody = body;
            this.Base = Proto == null ? null : Proto.Var;
            this.Object = new List<Variable>();
            this.IsSuper = false;
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
            for(int i = 0; i < ArgNames.Count; i++) {
                IValue? host = null;
                if(args.Count > i) {
                    host = args[i].Var;
                } else {
                    IOperator? def = Defaults[i];
                    if(def != null) {
                        host = def._Run().Var;
                    } else {
                        throw new RadishException($"The following required argument was unsupplied: {ArgNames[i]}");
                    }
                }
                Stack.Head.Val.Add(new Variable(ArgNames[i], host));
            }
            if(IsSuper) {
                //Console.WriteLine("calling super!");
                IValue? proto = ObjectLiteral.DeepGet(this, "prototype", this);
                if(proto != null) {
                    proto = proto.Var;
                    IValue? super = (proto.Base == null) ? null : Values.ObjectLiteral.DeepGet(proto.Base, "constructor", proto.Base); 
                    if(super != null) {
                        Stack.Head.Val.Add(new Variable("super", super.Var));
                    }
                }
                
            }
            IValue? saved = ObjectLiteral.CurrentPrivate;
            if(_this != null) {
                Stack.Head.Val.Add(new Variable("this", _this));
                ObjectLiteral.CurrentPrivate = _this;
            }
            string previous = RadishException.FileName;
            RadishException.FileName = FileName;
            IValue result = FunctionBody._Run();
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