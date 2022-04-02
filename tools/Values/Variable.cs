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
                if(Host == null) {
                    return BasicTypes.NONE;
                }
                return Host.Default;
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
        public List<Variable> Object {
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
        public IOperator FunctionBody { 
            get {
                return Resolve().FunctionBody;
            }
        }
        public IValue Clone() {
            return Resolve().Clone();
        }
        public bool Equals(IValue other) {
            return Resolve().Equals(other);
        }
    }
}