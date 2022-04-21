namespace Tools.Values {
    class NumberLiteral : EmptyLiteral {
        public override double Number { get; }
        public override IValue? Base { get; }
        public override List<Variable> Object { get; }
        public NumberLiteral(double input, IValue num) : base("number") {
            Number = input;
            Base = num;
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
            return Number == other.Number;
        }
        public override string Print() {
            return $"number({Number})";
        }
    }
}