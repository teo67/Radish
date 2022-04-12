namespace Tools.Operators {
    class Operator : IOperator {
        public IValue _Run() {
            try {
                return Run();
            } catch(RadishException error) {
                return OnError(error);
            } catch(Exception error) {
                return OnError(new RadishException(error.Message, Row, Col));
            }
        }
        public RadishException Error(string msg) {
            return new RadishException(msg, Row, Col);
        }
        public virtual string Print() {
            throw Error("Cannot print an operator!");
        }
        public virtual IValue OnError(RadishException error) {
            throw error;
        }
        public virtual IValue Run() {
            throw Error("Cannot run an operator!");
        }
        public virtual int Row { get; }
        public virtual int Col { get; }
        public Operator(int row, int col) {
            this.Row = row;
            this.Col = col;
        }
    }
}