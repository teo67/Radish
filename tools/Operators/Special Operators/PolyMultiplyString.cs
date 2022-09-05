namespace Tools.Operators {
    class PolyMultiplyString : SpecialOperator {
        public PolyMultiplyString(Librarian librarian) : base(librarian) {

        }
        public override IValue Run(Stack Stack) {
            string a = GetArgument(0)._Run(Stack).String;
            int b = (int)GetArgument(1)._Run(Stack).Number;
            string adding = "";
            for(int i = 0; i < b; i++) {
                adding += a;
            }
            return new Values.StringLiteral(adding);
        }
        public override string Print() {
            return $"({GetArgument(0).Print()} * {GetArgument(1).Print()})";
        }
    }
}