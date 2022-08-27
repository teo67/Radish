namespace Tools.Operators {
    class DirectoryDelete  : FileOperator {
        public DirectoryDelete(Librarian librarian) : base(librarian, -1, -1) {
        }
        public override IValue Run(Stack Stack) {
            string input = GetArgument(0)._Run(Stack).String;
            if(!Directory.Exists(input)) {
                throw new RadishException($"Directory {input} could not be deleted because it does not exist on the file system!", Row, Col);
            }
            Safe<bool?>(() => {
                File.Delete(input);
                return null;
            });
            return new Values.StringLiteral(input);
        }
        public override string Print() {
            return $"(delete directory {GetArgument(0).Print()})";
        }
    }
}