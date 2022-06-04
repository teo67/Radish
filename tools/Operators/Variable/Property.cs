namespace Tools.Operators {
    class Property : VariableOperator {
        private string Name { get; }
        private IOperator? Give { get; }
        private IOperator? Get { get; }
        private List<string> Modifiers { get; }
        public Property(Stack stack, string name, IOperator? give, IOperator? _get, List<string> modifiers, int row, int col) : base(stack, row, col) {
            this.Name = name;
            this.Give = give;
            this.Get = _get;
            this.Modifiers = modifiers;
        }
        public override IValue Run() {
            ProtectionLevels lvl = ProtectionLevels.PUBLIC;
            Dictionary<string, ProtectionLevels> dict = new Dictionary<string, ProtectionLevels>() {
                { "public",  ProtectionLevels.PUBLIC }, 
                { "private", ProtectionLevels.PRIVATE }, 
                { "protected", ProtectionLevels.PROTECTED }
            };
            bool isStatic = false;
            foreach(string modifier in Modifiers) {
                if(dict.Keys.Contains(modifier)) {
                    lvl = dict[modifier];
                } else if(modifier == "static") {
                    isStatic = true;
                }
            }

            foreach(Values.Variable variable in Stack.Head.Val) {
                if(variable.Name == Name && variable.IsStatic == isStatic) {
                    throw new RadishException($"Property {Name} already exists in the current scope!");
                }
            }
            
            Values.Variable adding = new Values.Property(Name, (Get == null) ? null : Get._Run(), (Give == null) ? null : Give._Run(), lvl, isStatic);
            Stack.Head.Val.Add(adding);
            return adding;
        }
        public override string Print() {
            return $"(declare property {Name})";
        }
    }
}