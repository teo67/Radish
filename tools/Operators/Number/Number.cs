namespace Tools.Operators {
    class Number : IOperator {
        private double Stored { get; }
        private IValue Num { get; }
        public Number(double stored, IValue num) {
            this.Stored = stored;
            this.Num = num;
        }
        public IValue Run() {
            return new Values.NumberLiteral(Stored, Num);
        }
        public string Print() {
            return $"{Stored}";
        }
    }
}