namespace Tools.Operators {
    class ClassDefinition : VariableOperator {
        private IOperator Body { get; }
        private string Inheriting { get; }
        public ClassDefinition(Stack stack, IOperator body, string inheriting, int row, int col) : base(stack, row, col) {
            this.Body = body;
            this.Inheriting = inheriting;
        }
        public override IValue Run() {
            Stack.Push();
            IValue result = Body._Run();
            if(result.Default == BasicTypes.RETURN) {
                throw new RadishException("Unexpected return statement inside a class definition!");
            }
            List<Values.Variable> popped = Stack.Pop().Val;
            IValue fun = new Values.FunctionLiteral(Stack, new List<string>(), new List<IOperator?>(), new Operators.ExpressionSeparator(Row, Col), Stack.Get("Function").Var);
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
                    }
                }
            }
            IValue fromStack = Stack.Get(Inheriting).Var;
            IValue? _base = Values.ObjectLiteral.Get(fromStack, "prototype", Stack, fromStack);
            if(_base == null) {
                _base = fromStack;
            }
            IValue proto = new Values.ObjectLiteral(nonstatics, _base);
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