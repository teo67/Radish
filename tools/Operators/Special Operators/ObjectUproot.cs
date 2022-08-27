namespace Tools.Operators {
    class ObjectUproot  : SpecialOperator {
        public ObjectUproot(Librarian librarian) : base(librarian) {
        }
        public override IValue Run(Stack Stack) {
            Dictionary<string, Values.Variable> obj = GetArgument(0)._Run(Stack).Object;
            string key = GetArgument(1)._Run(Stack).String;
            Values.Variable? saved = null;
            bool gotten = obj.TryGetValue(key, out saved);
            bool deleted = obj.Remove(key);
            if(gotten && deleted && saved != null) {
                return saved;
            }
            throw new RadishException($"No property {key} found to remove on given object!", Row, Col);
        }
        public override string Print() {
            return $"({GetArgument(0).Print} uproot {GetArgument(1).Print})";
        }
    }
}