namespace Tools.Values {
    class ArrayLiteral : IValue {
        private List<IValue> Stored { get; }
        public ArrayLiteral(List<IValue> input) {
            this.Stored = input;
        }
        public BasicTypes Default {
            get {
                return BasicTypes.ARRAY;
            }
        }
        public double Number {
            get {
                throw new Exception("Unable to parse array as number!");
            }
        }
        public string String {
            get {
                throw new Exception("Unable to parse array as string!");
            }
        }
        public bool Boolean {
            get {
                return Stored.Count > 0;
            }
        }
        public List<IValue> Array {
            get {
                return Stored;
            }
        }
        public Dictionary<string, IValue> Object {
            get {
                Dictionary<string, IValue> returning = new Dictionary<string, IValue>();
                for(int i = 0; i < Stored.Count; i++) {
                    returning[$"{i}"] = Stored[i];
                }
                return returning;
            }
        }
        public Carrier Var {
            get {
                throw new Exception("Could not use a literal as a variable!");
            }
        }
    }
}