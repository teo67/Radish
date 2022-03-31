namespace Tools.Values {
    class BooleanLiteral : EmptyLiteral {
        private bool Stored { get; set; }
        public BooleanLiteral(bool input) : base("boolean") {
            this.Stored = input;
        }
        public override  BasicTypes Default {
            get {
                return BasicTypes.BOOLEAN;
            }
        }
        public override double Number {
            get {
                return (Stored) ? 1 : 0;
            }
        }
        public override string String {
            get {
                return (Stored) ? "yes" : "no";
            }
        }
        public override bool Boolean {
            get {
                return Stored;
            }
        }
    }
}