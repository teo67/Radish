namespace Tools.Values {
    class BooleanLiteral : EmptyLiteral {
        public static IValue? Proto { private get; set; }
        public override bool Boolean { get; }
        public override IValue? Base { get; set; }
        public override Dictionary<string, Variable> Object { get; }
        public BooleanLiteral(bool input) : base("boolean") {
            this.Boolean = input;
            this.Base = Proto == null ? null : Proto.Var;
            this.Object = new Dictionary<string, Variable>();
        }
        public override  BasicTypes Default {
            get {
                return BasicTypes.BOOLEAN;
            }
        }
        public override double Number {
            get {
                return (Boolean) ? 1 : 0;
            }
        }
        public override string String {
            get {
                return (Boolean) ? "yes" : "no";
            }
        }
        public override bool Equals(IValue other) {
            return other.Default == BasicTypes.BOOLEAN && Boolean == other.Boolean;
        }
        public override string Print() {
            return $"boolean({Boolean})";
        }
    }
}