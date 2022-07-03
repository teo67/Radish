namespace Tools.Operators {
    class FileWrite : FileOperator {
        private IOperator Writing { get; }
        public FileWrite(IOperator fileName, IOperator writing) : base(fileName, -1, -1) {
            this.Writing = writing;
        }
        public override IValue Run(Stack Stack) {
            string path = FileName._Run(Stack).String;
            string dir = GetDirectory(path);
            if(!Directory.Exists(dir)) {
                throw new RadishException($"Directory {dir} does not exist! (while file.write can create a file, it cannot create a folder.)", Row, Col);
            }
            string writing = Writing._Run(Stack).String;
            Safe<bool?>(() => {
                File.WriteAllText(path, writing);
                return null;
            });
            return new Values.StringLiteral(writing);
        }
        public override string Print() {
            return $"(write file {FileName.Print()})";
        }
    }
}