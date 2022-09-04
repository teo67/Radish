namespace Tools.Operators {
    class RenderBMP  : FileOperator {
        public RenderBMP(Librarian librarian) : base(librarian, -1, -1) {
        }
        public override IValue Run(Stack Stack) {
            string res = GetArgument(0)._Run(Stack).String;
            byte[] decoded = System.Convert.FromBase64String(res);
            string path = GetArgument(1)._Run(Stack).String;
            if(!path.EndsWith(".bmp")) {
                path += ".bmp";
            }
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
            return $"renderbmp(to {GetArgument(1).Print()})";
        }
    }
}