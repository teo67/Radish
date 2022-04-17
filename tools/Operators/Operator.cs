namespace Tools.Operators {
    class Operator : IOperator {
        public IValue _Run() {
            int r = RadishException.Row;
            int c = RadishException.Col;
            RadishException.Row = this.Row;
            RadishException.Col = this.Col;
            IValue saved = Run();
            RadishException.Row = r;
            RadishException.Col = c;
            return saved;
        }
        public virtual string Print() {
            throw new RadishException("Cannot print an operator!");
        }
        public virtual IValue Run() {
            throw new RadishException("Cannot run an operator!");
        }
        public virtual int Row { get; }
        public virtual int Col { get; }
        public Operator(int row, int col) {
            this.Row = row;
            this.Col = col;
        }
    }
}