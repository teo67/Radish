namespace Tools.Operators {
    class Next : Operator {
        public Next() : base(-1, -1) {}
        public override IValue Run(Stack Stack) {
            string read = "" + Console.ReadKey(true).KeyChar;
            return new Values.StringLiteral(read);
        }
        public override string Print() {
            return "(read next press)";
        }
    }
}