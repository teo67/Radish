namespace Tools.Operators {
    class Clear : Operator {
        public Clear() : base(-1, -1) {}
        public override IValue Run(Stack Stack) {
            Console.Clear();
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return "(clear console)";
        }
    }
}