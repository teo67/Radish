namespace Tools.Values {
    class Property : Variable {
        private IValue? Get { get; }
        private IValue? Set { get; }
        public override IValue? Host {
            get {
                //Console.WriteLine($"TEST: accessing getter {Name}");
                return (Get == null) ? null : Get.Function(new List<IValue>());
            }
            protected set {
                //Console.WriteLine($"TEST: accessing setter {Name}");
                if(Set == null) {
                    throw new RadishException("No harvest function has been declared for this property!");
                }
                if(value == null) {
                    throw new RadishException("Unable to set a variable to no value (system error)!");
                }
                Set.Function(new List<IValue>() { value });
            }
        }

        public Property(IValue? _get, IValue? _set, ProtectionLevels protectionLevel = ProtectionLevels.PUBLIC, bool isStatic = false) : base(null, protectionLevel, isStatic, true) {
            this.Get = _get;
            this.Set = _set;
        }
        public override Variable Clone(IValue obj, string key) {
            return this;
        }
    }
}