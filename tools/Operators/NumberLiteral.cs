namespace Tools.Operators {
    class NumberLiteral : SimpleOperator<double, None, None> {
        private double Val { get; }
        public NumberLiteral(double val) : base(null, null) {
            this.Val = val;
        }
        public override double Run() {
            return Val;
        }
    }
}