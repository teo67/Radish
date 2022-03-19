namespace Tools.Values {
    class Variable : IValue {
        private Carrier Host { get; }
        public Variable() {
            this.Host = new Carrier();
        }
        private IValue Resolve() {
            if(Host.Carrying == null) {
                throw new Exception("No value stored in variable!");
            }
            return Host.Carrying;
        }
        public BasicTypes Default {
            get {
                return Resolve().Default;
            }
        }
        public double Number {
            get {
                return Resolve().Number;
            }
        }
        public string String {
            get {
                return Resolve().String;
            }
        }
        public bool Boolean {
            get {
                return Resolve().Boolean;
            }
        }
        public List<IValue> Array {
            get {
                return Resolve().Array;
            }
        }
        public Dictionary<string, IValue> Object {
            get {
                return Resolve().Object;
            }
        }
        public Carrier Var {
            get {
                return Host;
            }
        }
    }
}