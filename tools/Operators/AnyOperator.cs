namespace Tools.Operators {
    class AnyOperator : Operator {
        private List<IOperator> Args { get; }
        private Func<List<IOperator>, IValue> __Run { get; }
        private Func<List<IOperator>, string> _Print { get; }
        public AnyOperator(List<IOperator> args, Func<List<IOperator>, IValue> run, Func<List<IOperator>, string> print) : base(-1, -1) {
            this.Args = args;
            this.__Run = run;
            this._Print = print;
        }

        public override IValue Run() {
            return __Run(Args);
        }

        public override string Print() {
            return _Print(Args);
        }
    }
}