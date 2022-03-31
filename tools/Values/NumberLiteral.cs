namespace Tools.Values {
    class NumberLiteral : EmptyLiteral {
        public override double Number { get; }
        public override List<Variable> Object { get; }
        public NumberLiteral(double input, IValue num) : base("number") {
            Number = input;
            Object = new List<Variable>() {
                new Variable("base", num)
            };
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
    }
}