namespace Tools {
    class ProtoOperator {
        public Func<AbstractSyntaxNode, Boolean> IsDone { get; }
        public int NumArgs { get; }
        public ProtoOperator(Func<AbstractSyntaxNode, Boolean> isDone, int numArgs) {
            this.IsDone = isDone;
            this.NumArgs = numArgs;
        }
    }
}