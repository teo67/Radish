namespace Tools.Operators {
    class AnyOperator : IOperator {
        private List<IOperator> Args { get; }
        private Func<List<IOperator>, IValue> _Run { get; }
        private Func<List<IOperator>, string> _Print { get; }
        public AnyOperator(List<IOperator> args, Func<List<IOperator>, IValue> run, Func<List<IOperator>, string> print) {
            this.Args = args;
            this._Run = run;
            this._Print = print;
        }

        public IValue Run() {
            return _Run(Args);
        }

        public string Print() {
            return _Print(Args);
        }
    }
}