namespace Tools.Values {
    class ObjectLiteral : EmptyLiteral {
        protected List<Variable> Properties { get; }
        public ObjectLiteral(List<Variable> properties) : base("object") {
            this.Properties = properties;
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
        public override List<Variable> Object {
            get {
                return Properties;
            }
        }
        public static Variable Get(IValue target, string key) {
            IValue? proto = null;
            foreach(Variable property in target.Object) {
                if(property.Name == key) {
                    return property;
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