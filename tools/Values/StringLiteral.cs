namespace Tools.Values {
    class StringLiteral : EmptyLiteral {
        public override string String { get; }
        public override IValue? Base { get; }
        public override List<Variable> Object { get; }
        public StringLiteral(string input, IValue str) : base("string") {
            this.String = input;
            this.Base = str;
            this.Object = new List<Variable>();
        }
        public override BasicTypes Default {
            get {
                return BasicTypes.STRING;
            }
        }
        public override bool Boolean {
            get {
                return true;
            }
        }
        public override bool Equals(IValue other) {
            return other.Default == BasicTypes.STRING && String == other.String;
        }
        public override string Print() {
            return $"string({String})";
        }
    }
}