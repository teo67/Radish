namespace Tools.Values {
    class PropertyHolder : IValue {
        private IValue Held { get; }
        private string Name { get; }
        private IValue Obj { get; }
        private Stack Stack { get; }
        public PropertyHolder(IValue held, string name, IValue obj, Stack stack) {
            this.Held = held;
            this.Name = name;
            this.Obj = obj;
            this.Stack = stack;
        }
        public BasicTypes Default {
            get {
                return Held.Default;
            }
        }
        public double Number {
            get {
                return Held.Number;
            }
        }
        public string String {
            get {
                return Held.String;
            }
        }
        public bool Boolean {
            get {
                return Held.Boolean;
            }
        }
        public List<IValue> Array {
            get {
                return Held.Array;
            }
        }
        public List<Variable> Object {
            get {
                return Held.Object;
            }
        }
        public IValue Var {
            get {
                return Held;
            }
            set {
                foreach(Variable property in Obj.Object) {
                    if(property.Name == Name) {
                        property.Var = value;
                        return;
                    }
                }
                Obj.Object.Add(new Variable(Name, value));
            }
        }
        public IOperator FunctionBody {
            get {
                return Held.FunctionBody;
            }
        }
        public IValue Clone() {
            return new PropertyHolder(Held, Name, Obj, Stack);
        }
        public bool Equals(IValue other) {
            return Held.Equals(other);
        }
        public IValue Function(List<IValue> args) {
            Stack.Push(new List<Variable>() {
                new Variable("this", Obj)
            });
            IValue returned = Held.Function(args);
            Stack.Pop();
            return returned;
        }
    }
}