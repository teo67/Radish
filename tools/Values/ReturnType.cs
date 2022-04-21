namespace Tools.Values {
    class ReturnType : EmptyLiteral {
        private string Type { get; }
        private IValue Carrying { get; }
        public ReturnType(string type, IValue carrying) : base("return type") {
            this.Type = type;
            this.Carrying = carrying;
        }
        public override BasicTypes Default {
            get {
                return BasicTypes.RETURN;
            }
        }
        public override string String {
            get {
                return Type;
            }
        }
        public override IValue Function(List<Variable> args) {
            return Carrying;
        }
        public override string Print() {
            return $"{Type}({Carrying.Print()})";
        }
    }
}