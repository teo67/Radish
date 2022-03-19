namespace Tools.Values {
    class BooleanLiteral : IValue {
        private bool Stored { get; set; }
        public BooleanLiteral(bool input) {
            this.Stored = input;
        }
        public BasicTypes Default {
            get {
                return BasicTypes.BOOLEAN;
            }
        }
        public double Number {
            get {
                return (Stored) ? 1 : 0;
            }
        }
        public string String {
            get {
                return (Stored) ? "yes" : "no";
            }
        }
        public bool Boolean {
            get {
                return Stored;
            }
        }
        public List<IValue> Array {
            get {
                return new List<IValue>() { this };
            }
        }
        public Dictionary<string, IValue> Object {
            get {
                return new Dictionary<string, IValue>() { { "0", this } };
            }
        }
        public Carrier Var {
            get {
                throw new Exception("Could not use a literal as a variable!");
            }
        }
    }
}