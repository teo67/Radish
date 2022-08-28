namespace Tools.Operators {
    class EditPallette  : SpecialOperator {
        public EditPallette(Librarian librarian) : base(librarian) {
        }
        public override IValue Run(Stack Stack) {
            string input = GetArgument(0)._Run(Stack).String;
            byte[] decoded = Convert.FromBase64String(input);
            int palletteNumber = (int)GetArgument(1)._Run(Stack).Number;
            byte r = (byte)GetArgument(2)._Run(Stack).Number;
            byte g = (byte)GetArgument(3)._Run(Stack).Number;
            byte b = (byte)GetArgument(4)._Run(Stack).Number;
            decoded[palletteNumber * 4 + 56] = r;
            decoded[palletteNumber * 4 + 55] = g;
            decoded[palletteNumber * 4 + 54] = b;
            decoded[palletteNumber * 4 + 57] = 0;
            string encoded = Convert.ToBase64String(decoded);
            return new Values.StringLiteral(encoded);
        }
        public override string Print() {
            return $"editPallette({GetArgument(0).Print()}, {GetArgument(1).Print()}, {GetArgument(2).Print()}, {GetArgument(3).Print()}, {GetArgument(4).Print()})";
        }
    }
}