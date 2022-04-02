namespace Tools.Values {
    class BooleanLiteral : EmptyLiteral {
        public override bool Boolean { get; }
        public override List<Variable> Object { get; }
        public BooleanLiteral(bool input, IValue boo) : base("boolean") {
            this.Boolean = input;
            this.Object = new List<Variable>() {
                new Variable("base", boo)
            };
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
        public override IValue Clone() {
            return new BooleanLiteral(Boolean, ObjectLiteral.Get(this, "base"));
        }
        public override bool Equals(IValue other) {
            return Boolean == other.Boolean;
        }
    }
}