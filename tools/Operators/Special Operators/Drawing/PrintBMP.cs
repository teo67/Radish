namespace Tools.Operators {
    class PrintBMP : SpecialOperator {
        public PrintBMP(Librarian librarian) : base(librarian) {

        }
        public override IValue Run(Stack Stack) {
            byte[] printing = Convert.FromBase64String(GetArgument(0)._Run(Stack).String);
            foreach(byte by in printing) {
                Console.Write(by + " ");
            }
            Console.WriteLine("");
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return "(print bitmap)";
        }
    }
}