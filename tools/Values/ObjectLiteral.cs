namespace Tools.Values {
    class ObjectLiteral : EmptyLiteral {
        public override List<Variable> Object { get; }
        public override IValue? Base { get; }
        public ObjectLiteral(List<Variable> properties, IValue? _base = null) : base("object") {
            this.Object = properties;
            this.Base = _base;
        }
        public override BasicTypes Default {
            get {
                return BasicTypes.OBJECT;
            }
        }
        public override bool Boolean {
            get {
                return true;
            }
        }
        public override bool Equals(IValue other) {
            return other.Default == BasicTypes.OBJECT && Object == other.Object;
        }
        public static IValue? Get(IValue target, string key, Stack stack, IValue originalTarget) {
            foreach(Variable property in target.Object) {
                //Console.WriteLine(property.Name);
                if(property.Name == key) {
                    stack.Push(new List<Variable>() { // in case of setter function
                            new Variable("this", originalTarget)
                    });
                    IValue? reported = property.Host; // save host to ivalue and return, voila
                    //Console.WriteLine(key);
                    //Console.WriteLine(reported);
                    stack.Pop();
                    // if(key == "Name") {
                    //     throw new Exception("a");
                    // }
                    return reported;
                }
            }
            if(target.Base == null) {
                return null;
            }
            return Get(target.Base, key, stack, originalTarget);
        }
        public static bool ValidateArray(IValue target) {
            IValue _target = target.Var;
            for(int i = 0; i < _target.Object.Count; i++) {
                if(_target.Object[i].Name != $"{i}") {
                    return false;
                }
            }
            return true;
        }
    }
}