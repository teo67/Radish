namespace Tools.Values {
    class StringLiteral : EmptyLiteral {
        public override string String { get; }
        public override List<Variable> Object { get; }
        public StringLiteral(string input, IValue str) : base("string") {
            this.String = input;
            this.Object = new List<Variable>() {
                new Variable("base", str)
            };
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
    }
}