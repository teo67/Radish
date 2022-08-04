namespace Tools.Values {
    class ObjectLiteral : EmptyLiteral {
        public static IValue? Proto { private get; set; }
        public static IValue? ArrayProto { get; set; }
        public override Dictionary<string, Variable> Object { get; }
        public override IValue? Base { get; set; }
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
        // result, result holder
        public static (Variable?, IValue?) DeepGet(IValue target, string key, List<IValue> targetPath) {
            if(targetPath.Contains(target)) {
                return (null, null);
            }
            Variable? property = null;
            bool gotten = target.Object.TryGetValue(key, out property);
            if(gotten && property != null) {
                if(property.ProtectionLevel == ProtectionLevels.PUBLIC || (ObjectLiteral.CurrentPrivate != null && ((ObjectLiteral.CurrentPrivate.Var == target) || (targetPath.Contains(ObjectLiteral.CurrentPrivate.Var) && property.ProtectionLevel != ProtectionLevels.PRIVATE)))) {
                    return (property, target);
                }
            }
            if(target.Base == null) {
                return (null, null);
            }
            targetPath.Add(target);
            return DeepGet(target.Base.Var, key, targetPath);
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