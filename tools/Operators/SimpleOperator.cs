namespace Tools.Operators {
    class SimpleOperator<T, U, V> : IOperator<T> {
        public virtual IOperator<U>? Left { get; }
        public virtual IOperator<V>? Right { get; }
        public SimpleOperator(IOperator<U>? left, IOperator<V>? right) {
            this.Left = left;
            this.Right = right;
        }
        public virtual T Run() {
            throw new Exception("Could not run a SimpleOperator!");
        }
    }
}