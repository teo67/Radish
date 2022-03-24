namespace Tools.Values {
    class Variable : IValue {
        private IValue? Host { get; set; }
        public string Name { get; }
        public Variable(string name, IValue? host = null) {
            this.Name = name;
            this.Host = host;
        }
        private IValue Resolve() {
            if(Host == null) {
                throw new Exception("No value stored in variable!");
            }
            return Host;
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
        public IValue Var {
            get {
                return Resolve();
            }
            set {
                Host = value;
            }
        }
        public IValue Function(List<IValue> args) {
            return Resolve().Function(args);
        }
    }
}