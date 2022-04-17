namespace Tools.Operators {
    class Declaration : VariableOperator {
        private string VarName { get; }
        private List<string> Modifiers { get; }
        public Declaration(Stack stack, string varName, List<string> modifiers, int row, int col) : base(stack, row, col) {
            this.VarName = varName;
            this.Modifiers = modifiers;
        }
        public override IValue Run() {
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


            foreach(Values.Variable variable in Stack.Head.Val) {
                if(variable.Name == VarName && variable.IsStatic == isStatic) {
                    throw new RadishException($"{VarName} already exists in the current scope!");
                }
            }
            
            Values.Variable adding = new Values.Variable(VarName, null, lvl, isStatic);
            Stack.Head.Val.Add(adding);
            return adding;
        }
        public override string Print() {
            return $"(declare variable {VarName})";
        }
    }
}








