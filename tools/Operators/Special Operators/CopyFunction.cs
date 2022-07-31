namespace Tools.Operators {
    class CopyFunction : Operator {
        private IOperator Fun { get; }
        public CopyFunction(IOperator fun) : base(-1, -1) {
            Fun = fun;
        }
        public override IValue Run(Stack Stack) {
            IValue? ran = Fun._Run(Stack).Var;
            return new Values.FunctionLiteral(ran.Function); // i am cheating lol
        }
        public override string Print() {
            return "anonymous function";
        }
    }
}