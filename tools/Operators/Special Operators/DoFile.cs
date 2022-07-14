using System.Text;
namespace Tools.Operators {
    class DoFile : FileOperator {
        private IOperator Edits { get; }
        private IOperator ShouldRead { get; }
        public DoFile(IOperator fileName, IOperator shouldRead, IOperator edits) : base(fileName, -1, -1) {
            this.Edits = edits;
            this.ShouldRead = shouldRead;
        }
        public override IValue Run(Stack Stack) {
            string fileName = FileName._Run(Stack).String;
            if(!File.Exists(fileName)) {
                throw new RadishException($"File {fileName} does not exist on the file system!", Row, Col);
            }
            FileStream file = Safe<FileStream>(() => {
                return File.Open(fileName, FileMode.Open);
            });
            StreamReader reader = new StreamReader(file);
            List<IValue> args = new List<IValue>();
            if(ShouldRead._Run(Stack).Boolean) {
                args.Add(new Values.Variable(new Values.StringLiteral(reader.ReadToEnd())));
            }
            IValue edits = Edits._Run(Stack).Var;
            IValue edited = edits.Function(args, null).Var;
            IValue? delete = null;
            if(edited.Default != BasicTypes.NONE) {
                IValue? str = Values.ObjectLiteral.DeepGet(edited, "write", edited);
                delete = Values.ObjectLiteral.DeepGet(edited, "deleteFile", edited);
                IValue? append = Values.ObjectLiteral.DeepGet(edited, "append", edited);
                if(append != null && append.Var.Default != BasicTypes.NONE) {
                    file.Seek(0, SeekOrigin.End);
                    file.Write(Encoding.ASCII.GetBytes(append.String));
                } else if(str != null && str.Var.Default != BasicTypes.NONE) {
                    file.SetLength(0);
                    file.Write(Encoding.ASCII.GetBytes(str.String));
                }
            }
            reader.Dispose();
            file.Dispose();
            file.Close();
            if(delete != null && delete.Boolean) {
                Safe<bool?>(() => {
                    File.Delete(fileName);
                    return null;
                });
            }
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return $"(edit file {FileName})";
        }
    }
}