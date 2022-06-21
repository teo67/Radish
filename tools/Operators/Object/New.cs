namespace Tools.Operators {
    class New : Operator {
        private IOperator Args { get; }
        private IOperator ClassName { get; }
        public New(IOperator className, IOperator args, int row, int col) : base(row, col) {
            this.ClassName = className;
            this.Args = args;
        }
        public override IValue Run() {
            List<Values.Variable> args = Args._Run().Object;
            IValue _class = ClassName._Run().Var;
            IValue? inheriting = null;
            IValue? returned = Values.ObjectLiteral.DeepGet(_class, "prototype", _class);
            if(returned != null) {
                inheriting = returned.Var;
            }
            IValue returning = new Values.ObjectLiteral(new List<Values.Variable>(), inheriting, useProto: inheriting == null);
            _class.Function(args, returning);
            return returning;
        }

        public override string Print() {
            return $"(new {ClassName.Print()} ({Args.Print()}))";
        }
    }
}