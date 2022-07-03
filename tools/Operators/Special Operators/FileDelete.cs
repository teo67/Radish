namespace Tools.Operators {
    class FileDelete : FileOperator {
        public FileDelete(IOperator input) : base(input, -1, -1) {
        }
        public override IValue Run(Stack Stack) {
            string input = FileName._Run(Stack).String;
            if(!File.Exists(input)) {
                throw new RadishException($"File {input} could not be deleted because it does not exist on the file system!", Row, Col);
            }
            Safe<bool?>(() => {
                File.Delete(input);
                return null;
            });
            
            return new Values.StringLiteral(input);
        }
        public override string Print() {
            return $"(delete file {FileName.Print()})";
        }
    }
}