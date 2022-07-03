namespace Tools.Operators {
    class FileRead : FileOperator {
        public FileRead(IOperator input) : base(input, -1, -1) {
        }
        public override IValue Run(Stack Stack) {
            string input = FileName._Run(Stack).String;
            if(!File.Exists(input)) {
                throw new RadishException($"File {input} does not exist on the file system!", Row, Col);
            }
            string read = Safe<string>(() => {
                return File.ReadAllText(input);
            });
            return new Values.StringLiteral(read);
        }
        public override string Print() {
            return $"(read file {FileName.Print()})";
        }
    }
}