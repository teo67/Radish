namespace Tools.Values {
    class ReturnType : IValue {
        private string Type { get; }
        private IValue Carrying { get; }
        public ReturnType(string type, IValue carrying) {
            this.Type = type;
            this.Carrying = carrying;
        }
        public BasicTypes Default {
            get {
                return BasicTypes.RETURN;
            }
        }
        public double Number {
            get {
                throw new Exception("This value is still returning!");
            }
        }
        public string String {
            get {
                return Type;
            }
        }
        public bool Boolean {
            get {
                throw new Exception("This value is still returning!");
            }
        }
        public List<IValue> Array {
            get {
                throw new Exception("This value is still returning!");
            }
        }
        public Dictionary<string, IValue> Object {
            get {
                throw new Exception("This value is still returning!");
            }
        }
        public IValue Var {
            get {
                throw new Exception("This value is still returning!");
            }
            set {
                throw new Exception("This value is still returning!");
            }
        }
        public IValue Function(List<IValue> args) {
            return Carrying;
        }
    }
}