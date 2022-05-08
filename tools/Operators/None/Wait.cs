namespace Tools.Operators {
    class Wait : Operator {
        private IOperator Time { get; }
        public Wait(IOperator time, int row, int col) : base(row, col) {
            this.Time = time;
        }
        public override IValue Run() {
            double result = Time._Run().Var.Number;
            DateTime goal = DateTime.UtcNow;
            goal = goal.AddMilliseconds(result);
            while(DateTime.UtcNow < goal) {}
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return $"(wait {Time.Print()})";
        }
    }
}