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
            Values.Variable constr = new Values.Variable("constructor", new Values.FunctionLiteral(new Stack(Stack.Head), new List<string>(), new List<IOperator?>(), false, new NullValue(Row, Col), FileName));
            Stack.Head.Val.Add(constr);
            IValue result = Body._Run(Stack);
            if(result.Default == BasicTypes.RETURN) {
                throw new RadishException("Unexpected return statement inside a class definition!");
            }
            List<Values.Variable> popped = Stack.Pop().Val;
            // empty constructor in case class has none
            List<Values.Variable> statics = new List<Values.Variable>();
            List<Values.Variable> nonstatics = new List<Values.Variable>();
            for(int i = 0; i < popped.Count; i++) {
                if(popped[i].IsStatic) {
                    statics.Add(popped[i]);
                } else {
                    if(popped[i] != constr) {
                        nonstatics.Add(popped[i]);
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
                IValue? _base = Values.ObjectLiteral.DeepGet(fromStack, "prototype", fromStack);
                if(_base == null) {
                    _base = fromStack;
                }
                proto = new Values.ObjectLiteral(nonstatics, _base);
            }
            foreach(Values.Variable _static in statics) {
                evaluatedConstr.Object.Add(_static);
            }
            evaluatedConstr.Object.Add(new Values.Variable("prototype", proto));
            return evaluatedConstr;
        }

        public override string Print() {
            return $"(class {Body.Print()})";
        }
    }
}