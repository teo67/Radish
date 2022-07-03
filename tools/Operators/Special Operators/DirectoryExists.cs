namespace Tools.Operators {
    class DirectoryExists : FileOperator {
        public DirectoryExists(IOperator input) : base(input, -1, -1) {
        }
        public override IValue Run(Stack Stack) {
            string input = FileName._Run(Stack).String;
            return new Values.BooleanLiteral(Directory.Exists(input));
        }
        public override string Print() {
            return $"(exist directory {FileName.Print()})";
        }
    }
}