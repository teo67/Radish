namespace Tools.Operators {
    class FileWrite  : FileOperator {
        public FileWrite(Librarian librarian) : base(librarian, -1, -1) {
        }
        public override IValue Run(Stack Stack) {
            string path = GetArgument(0)._Run(Stack).String;
            string dir = GetDirectory(path);
            if(!Directory.Exists(dir)) {
                throw new RadishException($"Directory {dir} does not exist! (while file.write can create a file, it cannot create a folder.)", Row, Col);
            }
            string writing = GetArgument(1)._Run(Stack).String;
            Safe<bool?>(() => {
                File.WriteAllText(path, writing);
                return null;
            });
            return new Values.StringLiteral(writing);
        }
        public override string Print() {
            return $"(write file {GetArgument(0).Print()})";
        }
    }
}