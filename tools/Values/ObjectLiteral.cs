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
        public static IValue? Get(IValue target, string key) {
            foreach(Variable property in target.Object) {
                if(property.Name == key) {
                    return property.Host;
                }
            }
            if(target.Base == null) {
                return null;
            }
            return Get(target.Base, key);
        }
        public static bool ValidateArray(IValue target) {
            for(int i = 0; i < target.Object.Count; i++) {
                if(target.Object[i].Name != $"{i}") {
                    return false;
                }
            }
            return true;
        }
    }
}