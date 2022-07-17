namespace Tools.Operators {
    class FromJSON : Operator {
        private IOperator Input { get; }
        public FromJSON(IOperator input) : base(-1, -1) {
            Input = input;
        }
        public override IValue Run(Stack Stack) {
            string retr = Input._Run(Stack).String;
            JSONParser jsp = new JSONParser(retr);
            return jsp.Parse();
        }
        public override string Print() {
            return $"({Input.Print()} from json)";
        }
    }
}