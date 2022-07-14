namespace Tools.Operators {
    class Declaration : Operator {
        private string VarName { get; }
        private List<string> Modifiers { get; }
        public Declaration(string varName, List<string> modifiers, int row, int col) : base(row, col) {
            this.VarName = varName;
            this.Modifiers = modifiers;
        }
        public override IValue Run(Stack Stack) {
            ProtectionLevels lvl = ProtectionLevels.PUBLIC;
            Dictionary<string, ProtectionLevels> dict = new Dictionary<string, ProtectionLevels>() {
                { "public",  ProtectionLevels.PUBLIC }, 
                { "private", ProtectionLevels.PRIVATE }, 
                {"protected", ProtectionLevels.PROTECTED }
            };
            bool isStatic = false;
            foreach(string modifier in Modifiers) {
                if(dict.Keys.Contains(modifier)) {
                    lvl = dict[modifier];
                } else if(modifier == "static") {
                    isStatic = true;
                }
            }

            Values.Variable? o  = null;
            bool gotten = Stack.Head.Val.TryGetValue(VarName, out o);
            if(gotten && o != null && o.IsStatic == isStatic) {
                throw new RadishException($"{VarName} already exists in the current scope!");
            }
            
            Values.Variable adding = new Values.Variable(null, lvl, isStatic);
            Stack.Head.Val.Add(VarName, adding);
            return adding;
        }
        public override string Print() {
            return $"(declare variable {VarName})";
        }
    }
}








