namespace Tools.Values {
    class PropertyHolder : IValue {
        private Variable? Held { get; set; }
        private string Name { get; }
        private IValue Obj { get; }
        private IValue? RealHolder { get; }
        private ProtectionLevels ProtectionLevel { get; }
        public PropertyHolder(Variable? held, string name, IValue obj, IValue? realHolder, ProtectionLevels protectionLevel) {
            //Console.WriteLine($"creating property holder with value {held}, name {name}");
            this.Held = held;
            this.Name = name;
            this.Obj = obj;
            this.RealHolder = realHolder;
            this.ProtectionLevel = protectionLevel;
        }

        private IValue Resolve() {
            if(Held == null) {
                throw new RadishException($"No value stored in object property {Name}!");
            }
            IValue? ct = Values.ObjectLiteral.CurrentThis;
            Values.ObjectLiteral.CurrentThis = Obj;
            IValue saved = Held.Var;
            Values.ObjectLiteral.CurrentThis = ct;
            return saved;
        }

        public BasicTypes Default {
            get {
                return BasicTypes.NONE;
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
            }
            set {
                Resolve().Base = value;
            }
        }
        public IValue Var {
            get {
                return Resolve().Var;
            }
            set {
                if(Held == null) {
                    Values.Variable newv = new Variable(value, ProtectionLevel);
                    Held = newv;
                    Obj.Object.Add(Name, newv);
                } else {
                    Variable? setting = null;
                    bool gotten = Obj.Object.TryGetValue(Name, out setting);
                    if(!gotten || setting == null) {
                        setting = Held.Clone(Obj, Name);
                        Held = setting;
                    }
                    IValue? ct = ObjectLiteral.CurrentThis;
                    ObjectLiteral.CurrentThis = Obj;
                    setting.Var = value;
                    ObjectLiteral.CurrentThis = ct;
                }
            }
        }
        public bool Equals(IValue other) {
            return Resolve().Equals(other);
        }
        public Func<List<IValue>, IValue> Function {
            get {
                return (List<IValue> li) => {
                    IValue? ct = ObjectLiteral.CurrentThis;
                    ObjectLiteral.CurrentThis = Obj;
                    IValue res = Resolve().Function(li);
                    ObjectLiteral.CurrentThis = ct;
                    return res;
                };
            }
        }
        public string Print() {
            return $"propertyholder({(Held == null ? "null" : Held.Print())})";
            //return $"propertyholder(held = ({(Held == null ? "null" : Held.Print())}), obj = ({Obj.Print()}))";
        }
    }
}