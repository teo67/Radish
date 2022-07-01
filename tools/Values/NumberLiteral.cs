namespace Tools.Values {
    class NumberLiteral : EmptyLiteral {
        public static IValue? Proto { private get; set; }
        public override double Number { get; }
        public override IValue? Base { get; }
        public override List<Variable> Object { get; }
        public NumberLiteral(double input) : base("number") {
            Number = input;
            Base = Proto == null ? null : Proto.Var;
            Object = new List<Variable>();
        }
        public override BasicTypes Default {
            get {
                return BasicTypes.NUMBER;
            }
        }
        public override string String {
            get {
                return $"{Number}";
            }
        }
        public override bool Boolean {
            get {
                return Number != 0.0;
            }
        }
        public override bool Equals(IValue other) {
            return other.Default == BasicTypes.NUMBER && Number == other.Number;
        }
        public override string Print() {
            return $"number({Number})";
        }
    }
}