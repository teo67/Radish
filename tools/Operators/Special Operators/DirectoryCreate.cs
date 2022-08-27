namespace Tools.Operators {
    class DirectoryCreate : FileOperator {
        public DirectoryCreate(Librarian librarian) : base(librarian, -1, -1) {
        }
        public override IValue Run(Stack Stack) {
            string input = GetArgument(0)._Run(Stack).String;
            if(input.EndsWith('/')) {
                input = input.Substring(0, input.Length - 1);
            }
            string enclosing = GetDirectory(input);
            if(!Directory.Exists(enclosing)) {
                throw new RadishException($"Cannot create a directory at {input} because {enclosing} does not exist!", Row, Col);
            }
            bool exists = Directory.Exists(input);
            if(!exists) {
                Safe<bool?>(() => {
                    Directory.CreateDirectory(input);
                    return null;
                });
            }
            return new Values.BooleanLiteral(!exists);
        }
        public override string Print() {
            return $"(create directory {GetArgument(0).Print()})";
        }
    }
}