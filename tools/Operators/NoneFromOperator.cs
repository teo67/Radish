namespace Tools.Operators {
    class NoneFromOperator<T, U, V> : SimpleOperator<None, U, V> {
        private Func<T> run { get; }
        public NoneFromOperator(SimpleOperator<T, U, V> source) : base(source.Left, source.Right) {
            this.run = source.Run;
        }
        public override None Run() {
            run();
            return new None();
        }
    }
}