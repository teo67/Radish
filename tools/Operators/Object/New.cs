namespace Tools.Operators {
    class New : VariableOperator {
        private IOperator Args { get; }
        private IOperator ClassName { get; }
        public New(Stack stack, IOperator className, IOperator args, int row, int col) : base(stack, row, col) {
            this.ClassName = className;
            this.Args = args;
        }
        public override IValue Run() {
            List<Values.Variable> args = Args._Run().Object;
            IValue _class = ClassName._Run().Var;
            IValue inheriting = Stack.Get("Object").Var;
            IValue? returned = Values.ObjectLiteral.DeepGet(_class, "prototype", Stack, _class);
            if(returned != null) {
                inheriting = returned.Var;
            }
            IValue returning = new Values.ObjectLiteral(new List<Values.Variable>(), inheriting);
            _class.Function(args, returning);
            return returning;
        }

        public override string Print() {
            return $"(new {ClassName.Print()} ({Args.Print()}))";
        }
    }
}