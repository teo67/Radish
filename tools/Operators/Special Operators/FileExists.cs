namespace Tools.Operators {
    class FileExists : FileOperator {
        public FileExists(IOperator input) : base(input, -1, -1) {
        }
        public override IValue Run(Stack Stack) {
            string input = FileName._Run(Stack).String;
            return new Values.BooleanLiteral(File.Exists(input));
        }
        public override string Print() {
            return $"(exist file {FileName.Print()})";
        }
    }
}