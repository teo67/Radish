namespace Tools.Values {
    class PropertyHolder : IValue {
        private Variable? Held { get; }
        private string Name { get; }
        private IValue Obj { get; }
        private Stack Stack { get; }
        private ProtectionLevels ProtectionLevel { get; }
        public PropertyHolder(Variable? held, string name, IValue obj, Stack stack, ProtectionLevels protectionLevel) {
            //Console.WriteLine($"creating property holder with value {held}, name {name}");
            this.Held = held;
            this.Name = name;
            this.Obj = obj;
            this.Stack = stack;
            this.ProtectionLevel = protectionLevel;
        }

        public bool IsSuper {
            get {
                return Resolve().IsSuper;
            }
            set {
                Resolve().IsSuper = value;
            }
        }

        private IValue Resolve() {
            if(Held == null) {
                throw new RadishException($"No value stored in object property {Name}!");
            }
            IValue? previous = Held.ThisRef;
            Held.ThisRef = Obj;
            IValue saved = Held.Var;
            Held.ThisRef = previous;
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
                if(Held == null) {
                    Obj.Object.Add(new Variable(Name, value, ProtectionLevel));
                } else {
                    IValue? previous = Held.ThisRef;
                    Held.ThisRef = Obj;
                    Held.Var = value;
                    Held.ThisRef = previous;
                }
            }
        }
        public IOperator FunctionBody {
            get {
                return Resolve().FunctionBody;
            }
        }
        public bool Equals(IValue other) {
            return Resolve().Equals(other);
        }
        public IValue Function(List<Variable> args, IValue? _this) {
            //return Resolve().Function(args);
            IValue returned = Resolve().Function(args, Obj);
            return returned;
        }
        public string Print() {
            return $"propertyholder({(Held == null ? "null" : Held.Print())})";
            //return $"propertyholder(held = ({(Held == null ? "null" : Held.Print())}), obj = ({Obj.Print()}))";
        }
    }
}