namespace Tools.Values {
    class Variable : IValue {
        public virtual IValue? Host { get; protected set; }
        public ProtectionLevels ProtectionLevel { get; }
        public bool IsStatic { get; }
        public Variable(IValue? host = null, ProtectionLevels protectionLevel = ProtectionLevels.PUBLIC, bool isStatic = false, bool ignoreHost = false) {
            if(!ignoreHost) {
                this.Host = host;
            }
            this.ProtectionLevel = protectionLevel;
            this.IsStatic = isStatic;
        }
        private IValue Resolve() {
            IValue? returned = Host;
            if(returned == null) {
                throw new RadishException($"No value stored in variable!");
            }
            return returned; // store in a variable so we only call getter once
        }
        public BasicTypes Default {
            get {
                return BasicTypes.NONE; // in order to get the true value of a variable, you have to do .Var.Default
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
        public Dictionary<string, Variable> Object {
            get {
                return Resolve().Object;
            }
        }
        public IValue? Base {
            get {
                return Resolve().Base;
            } set {
                Resolve().Base = value;
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
        public Func<List<IValue>, IValue> Function {
            get {
                return Resolve().Function;
            }
        }
        public bool Equals(IValue other) {
            return Resolve().Equals(other);
        }
        public string Print() {
            return $"variable({(Host == null ? "null" : Host.Print())})";
        }
        public virtual Variable Clone(IValue obj, string key) {
            Variable returning = new Variable(Host, ProtectionLevel, IsStatic);
            obj.Object.Add(key, returning);
            return returning;
        }
    }
}