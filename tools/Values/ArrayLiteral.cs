namespace Tools.Values {
    class ArrayLiteral : EmptyLiteral {
        public override List<IValue> Array { get; }
        public override List<Variable> Object { get; }
        public ArrayLiteral(List<IValue> input, IValue arr) : base("array") {
            this.Array = input;
            this.Object = new List<Variable>() {
                new Variable("base", arr)
            };
        }
        public override BasicTypes Default {
            get {
                return BasicTypes.ARRAY;
            }
        }
        public override bool Boolean {
            get {
                return Array.Count > 0;
            }
        }
        public override IValue Clone() {
            return new ArrayLiteral(Array, ObjectLiteral.Get(this, "base"));
        }
        public override bool Equals(IValue other) {
            return Array == other.Array;
        }
    }
}