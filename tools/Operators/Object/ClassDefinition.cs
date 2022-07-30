namespace Tools.Operators {
    class ClassDefinition : Operator {
        private IOperator Body { get; }
        private IOperator? Inheriting { get; }
        private string FileName { get; }
        public ClassDefinition(IOperator body, IOperator? inheriting, int row, int col, string fileName) : base(row, col) {
            this.Body = body;
            this.Inheriting = inheriting;
            this.FileName = fileName;
        }
        public override IValue Run(Stack Stack) {
            Stack.Push();
            Values.Variable constr = new Values.Variable(new Values.FunctionLiteral(new Stack(Stack.Head), new List<string>(), new List<IOperator?>(), false, new NullValue(Row, Col), FileName));
            Stack.Head.Val.Add("constructor", constr);
            IValue result = Body._Run(Stack);
            if(result.Default == BasicTypes.RETURN) {
                throw new RadishException("Unexpected return statement inside a class definition!");
            }
            Dictionary<string, Values.Variable> popped = Stack.Pop().Val;
            // empty constructor in case class has none
            Dictionary<string, Values.Variable> statics = new Dictionary<string, Values.Variable>();
            Dictionary<string, Values.Variable> nonstatics = new Dictionary<string, Values.Variable>();
            foreach(KeyValuePair<string, Values.Variable> vari in popped) {
                if(vari.Value.IsStatic) {
                    statics.Add(vari.Key, vari.Value);
                } else {
                    if(vari.Value != constr) {
                        nonstatics.Add(vari.Key, vari.Value);
                    }
                }
            }
            IValue evaluatedConstr = constr.Var;
            IValue proto;
            if(Inheriting == null) {
                proto = new Values.ObjectLiteral(nonstatics, useProto: true);
            } else {
                IValue fromStack = Inheriting._Run(Stack).Var;
                evaluatedConstr.IsSuper = fromStack;
                IValue? _base = Values.ObjectLiteral.DeepGet(fromStack, "prototype", new List<IValue>()).Item1;
                if(_base == null) {
                    _base = fromStack;
                }
                proto = new Values.ObjectLiteral(nonstatics, _base);
            }
            foreach(KeyValuePair<string, Values.Variable> _static in statics) {
                evaluatedConstr.Object.Add(_static.Key, _static.Value);
            }
            evaluatedConstr.Object.Add("prototype", new Values.Variable(proto));
            return evaluatedConstr;
        }

        public override string Print() {
            return $"(class {Body.Print()})";
        }
    }
}