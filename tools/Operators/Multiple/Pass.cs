namespace Tools.Operators {
    class Pass : Operator {
        private IOperator Target { get; }
        public Pass(IOperator target, int row, int col) : base(row, col) {
            this.Target = target;
        }
        public override IValue Run() {
            Task.Run(() => {
                Target._Run();
            });
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return $"(pass {Target.Print()})";
        }
    }
}