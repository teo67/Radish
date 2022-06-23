namespace Tools.Operators {
    class ObjectUproot : Operator {
        private IOperator Obj { get; }
        private IOperator Key { get; }
        public ObjectUproot(IOperator obj, IOperator key) : base(-1, -1) {
            this.Obj = obj;
            this.Key = key;
        }
        public override IValue Run() {
            List<Values.Variable> obj = Obj._Run().Object;
            string key = Key._Run().String;
            for(int i = 0; i < obj.Count; i++) {
                if(obj[i].Name == key) {
                    IValue saved = obj[i].Var;
                    obj.RemoveAt(i);
                    return saved;
                }
            }
            throw new RadishException($"No property {key} found to remove on given object!", Row, Col);
        }
        public override string Print() {
            return $"({Obj.Print} uproot {Key.Print})";
        }
    }
}