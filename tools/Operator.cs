namespace Tools {
    class Operator { // where T is return type
        public LexEntry Carrier { get; }
        public Func<AbstractSyntaxNode, Boolean> IsDone { get; }
        public int NumArgs { get; }
        public Operator(ProtoOperator proto, LexEntry carrier) {
            this.IsDone = proto.IsDone;
            this.NumArgs = proto.NumArgs;
            this.Carrier = carrier;
        }
    }
}