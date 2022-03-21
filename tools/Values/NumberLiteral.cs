namespace Tools.Values {
    class NumberLiteral : IValue {
        private double Stored { get; }
        public NumberLiteral(double input) {
            this.Stored = input;
        }
        public BasicTypes Default {
            get {
                return BasicTypes.NUMBER;
            }
        }
        public double Number {
            get {
                return Stored;
            }
        }
        public string String {
            get {
                return $"{Stored}";
            }
        }
        public bool Boolean {
            get {
                return Stored != 0.0;
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
        public void Function(List<IValue> args) {}
    }
}