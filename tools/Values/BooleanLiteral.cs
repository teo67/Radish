namespace Tools.Values {
    class BooleanLiteral : EmptyLiteral {
        public override bool Boolean { get; }
        public override IValue? Base { get; }
        public override List<Variable> Object { get; }
        public BooleanLiteral(bool input, IValue boo) : base("boolean") {
            this.Boolean = input;
            this.Base = boo;
            this.Object = new List<Variable>();
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