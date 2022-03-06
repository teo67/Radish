namespace Tools.Values {
    class NoneLiteral : IValue {
        public NoneLiteral() {}
        public BasicTypes Default {
            get {
                return BasicTypes.NONE;
            }
        }
        public double Number {
            get {
                throw new Exception("No value found to convert to number!");
            }
        }
        public string String {
            get {
                throw new Exception("No value found to convert to string!");
            }
        }
        public bool Boolean {
            get {
                throw new Exception("No value found to convert to boolean!");
            }
        }
        public List<IValue> Array {
            get {
                throw new Exception("No value found to convert to array!");
            }
        }
        public Dictionary<string, IValue> Object {
            get {
                throw new Exception("No value found to convert to object!");
            }
        }
    }
}