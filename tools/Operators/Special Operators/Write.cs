namespace Tools.Operators {
    class Write : Output {
        public Write(Librarian librarian) : base(librarian) {}
        public override IValue Run(Stack Stack) {
            Tools.IValue result = (new Operators.Reference("0", -1, -1, Librarian))._Run(Stack);
            Console.Write(CalcOutput(result.Var));
            return result;
        }
        public override string Print() {
            return $"(write {(new Operators.Reference("0", -1, -1, Librarian)).Print()})";
        }
    }
}