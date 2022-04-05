namespace Tools.Operators {
    class New : VariableOperator {
        private IOperator Args { get; }
        private string ClassName { get; }
        public New(Stack stack, string className, IOperator args) : base(stack) {
            this.ClassName = className;
            this.Args = args;
        }
        public override IValue Run() {
            IValue _class = Stack.Get(ClassName).Var;
            IValue inheriting = Stack.Get("Object").Var;
            IValue? returned = Values.ObjectLiteral.Get(_class, "prototype");
            if(returned != null) {
                inheriting = returned;
            }
            IValue? super = (inheriting.Base == null) ? null : Values.ObjectLiteral.Get(inheriting.Base, "constructor");
            IValue returning = new Values.ObjectLiteral(new List<Values.Variable>(), inheriting);
            Stack.Push();
            Stack.Head.Val.Add(new Values.Variable("this", returning));
            if(super != null) {
                Stack.Head.Val.Add(new Values.Variable("super", super));
            }
            _class.Function(Args.Run().Object);
            Stack.Pop();
            return returning;
        }

        public override string Print() {
            return $"(new {ClassName} ({Args.Print()}))";
        }
    }
}