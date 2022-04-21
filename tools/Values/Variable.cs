namespace Tools.Values {
    class Variable : IValue {
        public virtual IValue? Host { get; protected set; }
        public string Name { get; }
        public ProtectionLevels ProtectionLevel { get; }
        public bool IsStatic { get; }
        public Variable(string name, IValue? host = null, ProtectionLevels protectionLevel = ProtectionLevels.PUBLIC, bool isStatic = false, bool ignoreHost = false) {
            this.Name = name;
            if(!ignoreHost) {
                this.Host = host;
            }
            this.ProtectionLevel = protectionLevel;
            this.IsStatic = isStatic;
        }
        private IValue Resolve() {
            IValue? returned = Host;
            if(returned == null) {
                throw new RadishException($"No value stored in variable {Name}!");
            }
            return returned; // store in a variable so we only call getter once
        }
        public BasicTypes Default {
            get {
                return BasicTypes.NONE; // in order to get the true value of a variable, you have to do .Var.Default
                // IValue? returned = Host;
                // if(returned == null) {
                //     return BasicTypes.NONE;
                // }
                // return returned.Default; // ^
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
        public List<Variable> Object {
            get {
                return Resolve().Object;
            }
        }
        public IValue? Base {
            get {
                return Resolve().Base;
            }
        }
        public IValue Var {
            get {
                return Resolve().Var;
            }
            set {
                Host = value;
            }
        }
        public IValue Function(List<Variable> args) {
            return Resolve().Function(args);
        }
        public IOperator FunctionBody { 
            get {
                return Resolve().FunctionBody;
            }
        }
        public bool Equals(IValue other) {
            return Resolve().Equals(other);
        }
        public string Print() {
            return $"variable({Name}, {(Host == null ? "null" : Host.Print())})";
        }
    }
}