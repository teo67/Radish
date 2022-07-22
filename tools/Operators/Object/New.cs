namespace Tools.Operators {
    class New : Operator {
        private List<IOperator> Args { get; }
        private IOperator ClassName { get; }
        public New(IOperator className, List<IOperator> args, int row, int col) : base(row, col) {
            this.ClassName = className;
            this.Args = args;
        }
        public override IValue Run(Stack Stack) {
            List<IValue> args = new List<IValue>();
            foreach(IOperator arg in Args) {
                args.Add(arg._Run(Stack).Var);
            }
            IValue _class = ClassName._Run(Stack).Var;
            IValue? inheriting = null;
            IValue? returned = Values.ObjectLiteral.DeepGet(_class, "prototype", _class).Item1;
            if(returned != null) {
                inheriting = returned.Var;
            }
            IValue returning = new Values.ObjectLiteral(new Dictionary<string, Values.Variable>(), inheriting, useProto: inheriting == null);
            _class.Function(args, returning, inheriting);
            return returning;
        }
        public override string Print() {
            string returning = $"(new {ClassName.Print()}";
            foreach(IOperator op in Args) {
                returning += $"\n{op.Print()}";
            }
            returning += ")";
            return returning;
        }
    }
}