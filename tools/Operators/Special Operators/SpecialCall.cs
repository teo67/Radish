namespace Tools.Operators {
    class SpecialCall : SpecialOperator {
        public SpecialCall(Librarian librarian) : base(librarian) {
        }
        public override IValue Run(Stack Stack) {
            Dictionary<string, Values.Variable> ret = GetArgument(1)._Run(Stack).Object;
            List<IValue> passing = new List<IValue>();
            foreach(KeyValuePair<string, Values.Variable> pair in ret) {
                passing.Add(pair.Value.Var);
            }
            IValue left = GetArgument(0)._Run(Stack);
            IValue ran = GetArgument(2)._Run(Stack).Var;
            IValue returned = left.Function(passing, ran, ran);
            return returned;
        }
        public override string Print() {
            return $"{GetArgument(0).Print}({GetArgument(0).Print()})";
        }
    }
}