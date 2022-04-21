namespace Tools.Values {
    class NoneLiteral : EmptyLiteral {
        public NoneLiteral() : base("none") {}
        public override BasicTypes Default {
            get {
                return BasicTypes.NONE;
            }
        }
        public override bool Equals(IValue other) {
            return other.Default == BasicTypes.NONE;
        }
        public override string String {
            get {
                return "null";
            }
        }
        public override string Print() {
            return $"none";
        }
    }
}