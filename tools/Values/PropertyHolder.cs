namespace Tools.Values {
    class PropertyHolder : IValue {
        private IValue? Held { get; }
        private string Name { get; }
        private IValue Obj { get; }
        private Stack Stack { get; }
        public PropertyHolder(IValue? held, string name, IValue obj, Stack stack) {
            this.Held = held;
            this.Name = name;
            this.Obj = obj;
            this.Stack = stack;
        }

        private IValue Resolve() {
            if(Held == null) {
                throw new Exception($"No value stored in object property {Name}!");
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
                return this;
            }
            set {
                foreach(Variable property in Obj.Object) {
                    if(property.Name == Name) {
                        Stack.Push(new List<Variable>() { // in case of setter function
                            new Variable("this", Obj)
                        });
                        property.Var = value; // trigger setter of property / variable
                        Stack.Pop();
                        return;
                    }
                }
                Obj.Object.Add(new Variable(Name, value));
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
            Stack.Push(new List<Variable>() {
                new Variable("this", Obj)
            });
            IValue returned = Resolve().Function(args);
            Stack.Pop();
            return returned;
        }
    }
}