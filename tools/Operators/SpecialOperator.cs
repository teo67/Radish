namespace Tools.Operators {
    class SpecialOperator : Operator {
        protected Librarian Librarian { get; }
        public SpecialOperator(Librarian librarian) : base(-1, -1) {
            this.Librarian = librarian;
        }
        protected IOperator GetArgument(int argNum) {
            return new Operators.Reference($"{argNum}", -1, -1, Librarian);
        }
    }
}