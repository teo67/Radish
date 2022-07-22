namespace Tools.Operators {
    class Keys : Operator {
        private IOperator Target { get; }
        public Keys(IOperator target) : base(-1, -1) {
            this.Target = target;
        }
        public override IValue Run(Stack Stack) {
            Dictionary<string, Values.Variable> res = Target._Run(Stack).Object;
            Dictionary<string, Values.Variable> arr = new Dictionary<string, Values.Variable>();
            int current = 0;
            foreach(KeyValuePair<string, Values.Variable> vari in res) {
                arr[$"{current}"] = new Values.Variable(new Values.StringLiteral(vari.Key));
                current++;
            }
            return new Values.ObjectLiteral(arr, useArrayProto: true);
        }
        public override string Print() {
            return $"keys {Target.Print()}";
        }
    }
}