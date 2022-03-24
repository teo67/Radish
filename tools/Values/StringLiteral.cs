namespace Tools.Values {
    class StringLiteral : IValue {
        private string Stored { get; }
        public StringLiteral(string input) {
            this.Stored = input;
        }
        public BasicTypes Default {
            get {
                return BasicTypes.STRING;
            }
        }
        public double Number {
            get {
                throw new Exception("Cannot convert string to number!");
            }
        }
        public string String {
            get {
                return Stored;
            }
        }
        public bool Boolean {
            get {
                return true;
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
        public IValue Var {
            get {
                throw new Exception("Could not use a literal as a variable!");
            }
            set {
                throw new Exception("Could not use a literal as a variable!");
            }
        }
        public IValue Function(List<IValue> args) {
            return this;
        }
    }
}