namespace Tools.Values {
    class NoneLiteral : EmptyLiteral {
        public NoneLiteral() : base("none") {}
        public override BasicTypes Default {
            get {
                return BasicTypes.NONE;
            }
        }
        public override IValue Clone() {
            return new NoneLiteral();
        }
        public override bool Equals(IValue other) {
            return other.Default == BasicTypes.NONE;
        }
    }
}