namespace Tools.Operators {
    class Read  : Operator {
        public Read() : base(-1, -1) {}
        public override IValue Run(Stack Stack) {
            string? read = Console.ReadLine();
            return new Values.StringLiteral(read == null ? "" : read);
        }
        public override string Print() {
            return "(read console)";
        }
    }
}