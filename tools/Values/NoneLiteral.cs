namespace Tools.Values {
    class NoneLiteral : EmptyLiteral {
        public NoneLiteral() : base("none") {}
        public override BasicTypes Default {
            get {
                return BasicTypes.NONE;
            }
        }
    }
}