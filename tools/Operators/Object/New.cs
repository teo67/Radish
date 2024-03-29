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
            IValue? returned = Values.ObjectLiteral.DeepGet(_class, "prototype", new List<IValue>()).Item1;
            if(returned != null) {
                inheriting = returned.Var;
            }
            IValue returning = new Values.ObjectLiteral(new Dictionary<string, Values.Variable>(), inheriting, useProto: inheriting == null);
            IValue? ct = Values.ObjectLiteral.CurrentThis;
            Values.ObjectLiteral.CurrentThis = returning;
            IValue newd = _class.Function(args).Var;
            Values.ObjectLiteral.CurrentThis = ct;
            return newd.Default == BasicTypes.NONE ? returning : newd;
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