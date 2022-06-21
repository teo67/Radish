namespace Tools.Values {
    class ObjectLiteral : EmptyLiteral {
        public static IValue? Proto { private get; set; }
        public static IValue? ArrayProto { get; set; }
        public override List<Variable> Object { get; }
        public override IValue? Base { get; }
        public ObjectLiteral(List<Variable> properties, IValue? _base = null, bool useProto = false, bool useArrayProto = false) : base("object") {
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
            foreach(Variable property in target.Object) {
                //Console.WriteLine(property.Name);
                if(property.Name == key) {
                    // public = any access, current object = modifier doesn't matter, derived object = modifier doesn't matter as long as it isn't private
                    if(property.ProtectionLevel == ProtectionLevels.PUBLIC || (ObjectLiteral.CurrentPrivate != null && ((ObjectLiteral.CurrentPrivate == target) || (ObjectLiteral.CurrentPrivate.Base != null && ObjectLiteral.CurrentPrivate.Base == target) || (ObjectLiteral.CurrentPrivate == originalTarget && property.ProtectionLevel != ProtectionLevels.PRIVATE)))) {
                        return property;
                    }
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
            foreach(Variable var in Object) {
                returning += $"\n{var.Print()}";
            }
            returning += "\n)";
            return returning;
        }
    }
}