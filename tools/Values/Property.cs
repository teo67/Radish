namespace Tools.Values {
    class Property : Variable {
        private IValue? Get { get; }
        private IValue? Set { get; }
        public override IValue? Host {
            get {
                //Console.WriteLine($"TEST: accessing getter {Name}");
                return (Get == null) ? null : Get.Function(new List<Variable>());
            }
            protected set {
                //Console.WriteLine($"TEST: accessing setter {Name}");
                if(Set == null) {
                    throw new RadishException("No harvest function has been declared for this property!");
                }
                Set.Function(new List<Variable>() { new Variable("input", value) });
            }
        }

        public Property(string name, IValue? _get, IValue? _set, ProtectionLevels protectionLevel = ProtectionLevels.PUBLIC, bool isStatic = false) : base(name, null, protectionLevel, isStatic, true) {
            this.Get = _get;
            this.Set = _set;
        }
    }
}