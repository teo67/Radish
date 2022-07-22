namespace Tools.Operators {
    class _Values : Operator {
        private IOperator Target { get; }
        public _Values(IOperator target) : base(-1, -1) {
            this.Target = target;
        }
        public override IValue Run(Stack Stack) {
            Dictionary<string, Values.Variable> res = Target._Run(Stack).Object;
            Dictionary<string, Values.Variable> arr = new Dictionary<string, Values.Variable>();
            int current = 0;
            foreach(KeyValuePair<string, Values.Variable> vari in res) {
                arr[$"{current}"] = new Values.Variable(vari.Value.Var);
                current++;
            }
            return new Values.ObjectLiteral(arr, useArrayProto: true);
        }
        public override string Print() {
            return $"keys {Target.Print()}";
        }
    }
}