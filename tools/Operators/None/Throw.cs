namespace Tools.Operators {
    class Throw : VariableOperator {
        public IOperator Throwing { get; }
        public Throw(IOperator throwing, Stack stack, int row, int col) : base(stack, row, col) {
            this.Throwing = throwing;
        }
        public override IValue Run() {
            throw new RadishException(Throwing._Run().String, -1, -1);
        }
        public override string Print() {
            return $"(throw {Throwing.Print()})";
        }
    }
}