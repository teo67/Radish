namespace Tools.Operators {
    class Write : Output {
        public Write(IOperator input) : base(input) {}
        public override IValue Run(Stack Stack) {
            Tools.IValue result = Input._Run(Stack);
            Console.Write(CalcOutput(result.Var));
            return result;
        }
        public override string Print() {
            return $"(write {Input.Print()})";
        }
    }
}