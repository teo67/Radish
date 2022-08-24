namespace Tools.Operators {
    class RenderBMP : FileOperator {
        private IOperator Source { get; }
        public RenderBMP(IOperator source, IOperator outFile) : base(outFile, -1, -1) {
            Source = source;
        }
        public override IValue Run(Stack Stack) {
            string res = Source._Run(Stack).String;
            byte[] decoded = System.Text.Encoding.Unicode.GetBytes(res, 0, res.Length);
            Console.WriteLine(decoded.Length);
            foreach(byte by in decoded) {
                Console.Write(by + " ");
            }
            string path = FileName._Run(Stack).String;
            string dir = GetDirectory(path);
            if(!Directory.Exists(dir)) {
                throw new RadishException($"Directory {dir} does not exist! (while renderbmp can create a file, it cannot create a folder.)", Row, Col);
            }
            Safe<bool?>(() => {
                File.WriteAllBytes(path, decoded);
                return null;
            });
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return $"renderbmp(to {FileName.Print()})";
        }
    }
}