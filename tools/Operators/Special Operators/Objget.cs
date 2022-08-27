namespace Tools.Operators {
    class Objget  : SpecialOperator {
        public Objget(Librarian librarian) : base(librarian) {
        }
        public override IValue Run(Stack Stack) {
            IValue result = GetArgument(0)._Run(Stack).Var;
            Values.Variable? gotten = Values.ObjectLiteral.DeepGet(result, GetArgument(1)._Run(Stack).String, new List<IValue>()).Item1;
            return gotten == null ? new Values.NoneLiteral() : gotten.Var;
        }

        public override string Print() {
            return $"(get {GetArgument(0).Print()}.{GetArgument(1).Print()})";
        }
    }
}