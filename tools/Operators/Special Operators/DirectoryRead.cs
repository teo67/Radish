namespace Tools.Operators {
    class DirectoryRead : FileOperator {
        public DirectoryRead(IOperator input) : base(input, -1, -1) {
        }
        private void Iterate(Dictionary<string, Values.Variable> arr, string[] adding) {
            int i = arr.Count;
            foreach(string dir in adding) {
                string edited = dir.Replace('\\', '/');
                int j = edited.LastIndexOf('/');
                if(j != -1 && j < edited.Length - 1) {
                    edited = edited.Substring(j + 1);
                }
                arr.Add($"{i}", new Values.Variable(new Values.StringLiteral(edited)));
                i++;
            }
        }
        public override IValue Run(Stack Stack) {
            string input = FileName._Run(Stack).String;
            if(!Directory.Exists(input)) {
                throw new RadishException($"Directory {input} could not be read because it does not exist on the file system!", Row, Col);
            }
            Dictionary<string, Values.Variable> arr = new Dictionary<string, Values.Variable>();
            (string[], string[]) all = Safe<(string[], string[])>(() => {
                string[] allFiles = Directory.GetFiles(input);
                string[] allDirectories = Directory.GetDirectories(input);
                return (allFiles, allDirectories);
            });
            Iterate(arr, all.Item2);
            Iterate(arr, all.Item1);
            return new Values.ObjectLiteral(arr, useArrayProto: true);
        }
        public override string Print() {
            return $"(read directory {FileName.Print()})";
        }
    }
}