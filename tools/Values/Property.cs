namespace Tools.Values {
    class Property : Variable {
        private IValue? Get { get; }
        private IValue? Set { get; }
        public override IValue? Host {
            get {
                return (Get == null) ? null : Get.Function(new List<Variable>());
            }
            protected set {
                if(Set == null) {
                    throw new Exception("No setter function has been declared for this property!");
                }
                Set.Function(new List<Variable>() { new Variable("input", value) });
            }
        }

        public Property(string name, IValue? _get, IValue? _set) : base(name, null, true) {
            this.Get = _get;
            this.Set = _set;
        }
    }
}