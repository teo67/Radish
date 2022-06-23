namespace Tools.Operators {
    class ClassDefinition : VariableOperator {
        private IOperator Body { get; }
        private IOperator? Inheriting { get; }
        private string FileName { get; }
        public ClassDefinition(Stack stack, IOperator body, IOperator? inheriting, int row, int col, string fileName) : base(stack, row, col) {
            this.Body = body;
            this.Inheriting = inheriting;
            this.FileName = fileName;
        }
        public override IValue Run() {
            Stack.Push();
            IValue result = Body._Run();
            if(result.Default == BasicTypes.RETURN) {
                throw new RadishException("Unexpected return statement inside a class definition!");
            }
            List<Values.Variable> popped = Stack.Pop().Val;
            IValue? fun = null;
            // empty constructor in case class has none
            List<Values.Variable> statics = new List<Values.Variable>();
            List<Values.Variable> nonstatics = new List<Values.Variable>();
            for(int i = 0; i < popped.Count; i++) {
                if(popped[i].IsStatic) {
                    statics.Add(popped[i]);
                } else {
                    nonstatics.Add(popped[i]);
                    if(popped[i].Name == "constructor") {
                        fun = popped[i].Var;
                        fun.IsSuper = true;
                    }
                }
            }
            if(fun == null) {
                fun = new Values.FunctionLiteral(Stack, new List<string>(), new List<IOperator?>(), new Operators.ExpressionSeparator(Row, Col), FileName);
                nonstatics.Add(new Values.Variable("constructor", fun));
            }
            IValue proto;
            if(Inheriting == null) {
                proto = new Values.ObjectLiteral(nonstatics, useProto: true);
            } else {
                IValue fromStack = Inheriting._Run().Var;
                IValue? _base = Values.ObjectLiteral.DeepGet(fromStack, "prototype", fromStack);
                if(_base == null) {
                    _base = fromStack;
                }
                proto = new Values.ObjectLiteral(nonstatics, _base);
            }
            foreach(Values.Variable _static in statics) {
                fun.Object.Add(_static);
            }
            fun.Object.Add(new Values.Variable("prototype", proto));
            return fun;
        }

        public override string Print() {
            return $"(class {Body.Print()})";
        }
    }
}