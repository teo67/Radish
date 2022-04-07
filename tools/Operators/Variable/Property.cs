namespace Tools.Operators {
    class Property : VariableOperator {
        private string Name { get; }
        private IOperator? Give { get; }
        private IOperator? Get { get; }
        public Property(Stack stack, string name, IOperator? give, IOperator? _get) : base(stack) {
            this.Name = name;
            this.Give = give;
            this.Get = _get;
        }
        public override IValue Run() {
            foreach(Values.Variable variable in Stack.Head.Val) {
                if(variable.Name == Name) {
                    throw new Exception($"Could not redeclare property {Name}!");
                }
            }
            Values.Variable adding = new Values.Property(Name, (Get == null) ? null : Get.Run(), (Give == null) ? null : Give.Run());
            Stack.Head.Val.Add(adding);
            return adding;
        }
        public override string Print() {
            return $"(declare property {Name})";
        }
    }
}