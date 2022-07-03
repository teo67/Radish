namespace Tools.Operators {
    class FileOperator : Operator {
        public IOperator FileName { get; }
        public FileOperator(IOperator fileName, int row, int col) : base(row, col) {
            this.FileName = fileName;
        }
        protected string Convert(string path, Stack Stack) {
            string name = FileName._Run(Stack).String;
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