namespace Tools.Values {
    class PolyLiteral : EmptyLiteral {
        public static IValue? Proto { private get; set; }
        public override double Number { get; }
        public override bool Boolean { get; }
        public override string String { get; }
        public override IValue? Base { get; set; }
        public override Dictionary<string, Variable> Object { get; }
        public PolyLiteral(double num, bool boo, string str) : base("poly") {
            Number = num;
            Boolean = boo;
            String = str;
            Base = Proto == null ? null : Proto.Var;
            Object = new Dictionary<string, Variable>();
        }
        public override BasicTypes Default {
            get {
                return BasicTypes.POLY;
            }
        }
        public override bool Equals(IValue other) {
            return other.Default == BasicTypes.POLY && Number == other.Number && String == other.String && Boolean == other.Boolean;
        }
        public override string Print() {
            return $"poly({Number}, {String}, {Boolean})";
        }
    }
}