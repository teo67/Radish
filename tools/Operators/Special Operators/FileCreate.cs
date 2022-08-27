namespace Tools.Operators {
    class FileCreate  : FileOperator {
        public FileCreate(Librarian librarian) : base(librarian, -1, -1) {
        }
        public override IValue Run(Stack Stack) {
            string input = GetArgument(0)._Run(Stack).String;
            string enclosing = GetDirectory(input);
            if(!Directory.Exists(enclosing)) {
                throw new RadishException($"Cannot create a file at {input} because {enclosing} does not exist!", Row, Col);
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
            return $"(create file {GetArgument(0).Print()})";
        }
    }
}