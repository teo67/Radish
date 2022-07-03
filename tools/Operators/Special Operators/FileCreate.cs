namespace Tools.Operators {
    class FileCreate : FileOperator {
        public FileCreate(IOperator input) : base(input, -1, -1) {
        }
        public override IValue Run(Stack Stack) {
            string input = FileName._Run(Stack).String;
            string enclosing = GetDirectory(input);
            if(!Directory.Exists(enclosing)) {
                throw new RadishException($"Cannot create a file at {FileName} because {enclosing} does not exist!", Row, Col);
            }
            bool exists = File.Exists(input);
            if(!exists) {
                Safe<bool?>(() => {
                    File.Create(input);
                    return null;
                });
            }
            return new Values.BooleanLiteral(!exists);
        }
        public override string Print() {
            return $"(create file {FileName.Print()})";
        }
    }
}