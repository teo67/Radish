namespace Tools.Values {
    class ObjectLiteral : EmptyLiteral {
        public static IValue? Proto { private get; set; }
        public static IValue? ArrayProto { get; set; }
        public override Dictionary<string, Variable> Object { get; }
        public override IValue? Base { get; }
        public ObjectLiteral(Dictionary<string, Variable> properties, IValue? _base = null, bool useProto = false, bool useArrayProto = false) : base("object") {
            this.Object = properties;
            if(useProto) {
                Base = Proto == null ? null : Proto.Var;
            } else if(useArrayProto) {
                Base = ArrayProto == null ? null : ArrayProto.Var;
            } else {
                Base = _base;
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
        public override bool Equals(IValue other) {
            return other.Default == BasicTypes.OBJECT && Object == other.Object;
        }

        public static Variable? DeepGet(IValue target, string key, IValue originalTarget) {
            Variable? property = null;
            bool gotten = target.Object.TryGetValue(key, out property);
            if(gotten && property != null) {
                if(property.ProtectionLevel == ProtectionLevels.PUBLIC || (ObjectLiteral.CurrentPrivate != null && ((ObjectLiteral.CurrentPrivate == target) || (ObjectLiteral.CurrentPrivate.Base != null && ObjectLiteral.CurrentPrivate.Base == target) || (ObjectLiteral.CurrentPrivate == originalTarget && property.ProtectionLevel != ProtectionLevels.PRIVATE)))) {
                    return property;
                }
            }
            if(target.Base == null) {
                return null;
            }
            return DeepGet(target.Base, key, originalTarget);
        }

        public static IValue? CurrentPrivate { get; set; } // stores the object that is currently able to access private / protected properties
        public override string Print() {
            string returning = "object(";
            foreach(KeyValuePair<string, Variable> var in Object) {
                returning += $"\n{var.Key}: {var.Value}";
            }
            returning += "\n)";
            return returning;
        }
    }
}