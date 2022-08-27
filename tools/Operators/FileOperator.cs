namespace Tools.Operators {
    class FileOperator : Operator {
        protected Librarian Librarian { get; }
        public FileOperator(Librarian librarian, int row, int col) : base(row, row) {
            Librarian = librarian;
        }
        protected IOperator GetArgument(int argNum) {
            return new Operators.Reference($"{argNum}", -1, -1, Librarian);
        }
        protected string Convert(IOperator converting, string path, Stack Stack) {
            string name = converting._Run(Stack).String;
            string modPath = GetDirectory(path);
            while(name.StartsWith("../")) {
                modPath = GetDirectory(modPath);
                name = name.Substring(3);
            }
            return modPath + "/" + name;
        }
        protected string GetDirectory(string path) {
            int lastI = path.LastIndexOf('/');
            if(lastI == -1) {
                throw new RadishException($"File {path} escapes an invalid number of folders!", Row, Col);
            }
            return path.Substring(0, lastI);
        }
        protected T Safe<T>(Func<T> func) {
            try {
                return func();
            } catch(IOException) {
                throw new RadishException("The file or directory specified is already in use! It's possible that your computer is just going too fast!", Row, Col);
            } catch(UnauthorizedAccessException) {
                throw new RadishException("You do not have permission to access the file or directory specified!", Row, Col);
            }
        }
    }
}