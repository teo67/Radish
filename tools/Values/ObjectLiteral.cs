namespace Tools.Values {
    class ObjectLiteral : EmptyLiteral {
        public override List<Variable> Object { get; }
        public ObjectLiteral(List<Variable> properties, IValue? _base = null) : base("object") {
            this.Object = properties;
            if(_base != null) {
                this.Object.Add(new Variable("base", _base)); // ONLY DO THIS IF PROPERTIES ALREADY INCLUDES BASE
            }
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
        public override IValue Clone() {
            return new ObjectLiteral(Object);
        }
        public override bool Equals(IValue other) {
            return Object == other.Object;
        }
        public static IValue Get(IValue target, string key) {
            IValue? proto = null;
            foreach(Variable property in target.Object) {
                if(property.Name == key) {
                    return property.Var;
                }
                if(property.Name == "base") {
                    proto = property.Var;
                }
            }
            if(proto == null) {
                throw new Exception($"Could not find property {key}!");
            }
            return Get(proto, key);
        }
    }
}