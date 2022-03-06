namespace Tools.Operators {
    class Number : IOperator {
        private double Stored { get; }
        public Number(double stored) {
            this.Stored = stored;
        }
        public IValue Run() {
            return new Values.NumberLiteral(Stored);
        }
        public string Print() {
            return $"{Stored}";
        }
    }
}