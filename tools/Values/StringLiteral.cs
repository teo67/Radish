namespace Tools.Values {
    class StringLiteral : EmptyLiteral {
        public static IValue? Proto { private get; set; }
        public override string String { get; }
        public override IValue? Base { get; set; }
        public override Dictionary<string, Variable> Object { get; }
        public StringLiteral(string input) : base("string") {
            this.String = input;
            this.Base = Proto == null ? null : Proto.Var;
            this.Object = new Dictionary<string, Variable>();
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
        public override double Number {
            get {
                try {
                    return Double.Parse(String);
                } catch {
                    throw new RadishException("Unable to convert an invalid string into a number!");
                }
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