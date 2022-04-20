namespace Tools.Operators {
    class New : VariableOperator {
        private IOperator Args { get; }
        private string ClassName { get; }
        public New(Stack stack, string className, IOperator args, int row, int col) : base(stack, row, col) {
            this.ClassName = className;
            this.Args = args;
        }
        public override IValue Run() {
            List<Values.Variable> args = Args._Run().Object;
            IValue _class = Stack.Get(ClassName).Var;
            IValue inheriting = Stack.Get("Object").Var;
            IValue? returned = Values.ObjectLiteral.Get(_class, "prototype", Stack, _class);
            if(returned != null) {
                inheriting = returned;
            }
            IValue? super = (inheriting.Base == null) ? null : Values.ObjectLiteral.Get(inheriting.Base, "constructor", Stack, inheriting.Base);
            IValue returning = new Values.ObjectLiteral(new List<Values.Variable>(), inheriting);
            Stack.Push();
            Stack.Head.Val.Add(new Values.Variable("this", returning));
            IValue? saved = Values.ObjectLiteral.CurrentPrivate;
            Values.ObjectLiteral.CurrentPrivate = returning;
            if(super != null) {
                Stack.Head.Val.Add(new Values.Variable("super", super));
            }
            _class.Function(args);
            Stack.Pop();
            Values.ObjectLiteral.CurrentPrivate = saved;
            return returning;
        }

        public override string Print() {
            return $"(new {ClassName} ({Args.Print()}))";
        }
    }
}