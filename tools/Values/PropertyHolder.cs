namespace Tools.Values {
    class PropertyHolder : IValue {
        private IValue? Held { get; }
        private string Name { get; }
        private IValue Obj { get; }
        private Stack Stack { get; }
        private ProtectionLevels ProtectionLevel { get; }
        public PropertyHolder(IValue? held, string name, IValue obj, Stack stack, ProtectionLevels protectionLevel) {
            //Console.WriteLine($"creating property holder with value {held}, name {name}");
            this.Held = held;
            this.Name = name;
            this.Obj = obj;
            this.Stack = stack;
            this.ProtectionLevel = protectionLevel;
        }

        private IValue Resolve() {
            if(Held == null) {
                throw new RadishException($"No value stored in object property {Name}!");
            }
            return Held;
        }

        public BasicTypes Default {
            get {
                if(Held == null) {
                    return BasicTypes.NONE;
                }
                return Held.Default;
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
                foreach(Variable property in Obj.Object) {
                    if(property.Name == Name) {
                        Stack.Push(new List<Variable>() { // in case of setter function
                            new Variable("this", Obj)
                        });
                        IValue? saved = ObjectLiteral.CurrentPrivate;
                        ObjectLiteral.CurrentPrivate = Obj;
                        property.Var = value; // trigger setter of property / variable
                        Stack.Pop();
                        ObjectLiteral.CurrentPrivate = saved;
                        return;
                    }
                }
                //Console.WriteLine($"adding new property: {Name}");
                Obj.Object.Add(new Variable(Name, value, ProtectionLevel));
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
        public IValue Function(List<Variable> args) {
            //return Resolve().Function(args);
            Stack.Push(new List<Variable>() {
                new Variable("this", Obj)
            });
            IValue? saved = ObjectLiteral.CurrentPrivate;
            ObjectLiteral.CurrentPrivate = Obj;
            IValue returned = Resolve().Function(args);
            Stack.Pop();
            ObjectLiteral.CurrentPrivate = saved;
            return returned;
        }
        public string Print() {
            return $"propertyholder({(Held == null ? "null" : Held.Print())})";
            //return $"propertyholder(held = ({(Held == null ? "null" : Held.Print())}), obj = ({Obj.Print()}))";
        }
    }
}