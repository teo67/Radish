namespace Tools.Operators {
    class Throw : Operator {
        public IOperator Throwing { get; }
        public Throw(IOperator throwing, int row, int col) : base(row, col) {
            this.Throwing = throwing;
        }
        public override IValue Run(Stack Stack) {
            throw new RadishException(Throwing._Run(Stack).String, -1, -1);
        }
        public override string Print() {
            return $"(throw {Throwing.Print()})";
        }
    }
}