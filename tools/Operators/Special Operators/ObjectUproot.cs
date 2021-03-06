namespace Tools.Operators {
    class ObjectUproot : Operator {
        private IOperator Obj { get; }
        private IOperator Key { get; }
        public ObjectUproot(IOperator obj, IOperator key) : base(-1, -1) {
            this.Obj = obj;
            this.Key = key;
        }
        public override IValue Run(Stack Stack) {
            Dictionary<string, Values.Variable> obj = Obj._Run(Stack).Object;
            string key = Key._Run(Stack).String;
            Values.Variable? saved = null;
            bool gotten = obj.TryGetValue(key, out saved);
            bool deleted = obj.Remove(key);
            if(gotten && deleted && saved != null) {
                return saved;
            }
            throw new RadishException($"No property {key} found to remove on given object!", Row, Col);
        }
        public override string Print() {
            return $"({Obj.Print} uproot {Key.Print})";
        }
    }
}