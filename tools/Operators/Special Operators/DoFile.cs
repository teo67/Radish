using System.Text;
namespace Tools.Operators {
    class DoFile  : FileOperator {
        public DoFile(Librarian librarian) : base(librarian, -1, -1) {}
        public override IValue Run(Stack Stack) {
            string fileName = GetArgument(0)._Run(Stack).String;
            if(!File.Exists(fileName)) {
                throw new RadishException($"File {fileName} does not exist on the file system!", Row, Col);
            }
            FileStream file = Safe<FileStream>(() => {
                return File.Open(fileName, FileMode.Open);
            });
            StreamReader reader = new StreamReader(file);
            List<IValue> args = new List<IValue>();
            if(GetArgument(1)._Run(Stack).Boolean) { // should read
                args.Add(new Values.Variable(new Values.StringLiteral(reader.ReadToEnd())));
            }
            IValue edits = GetArgument(2)._Run(Stack).Var; // edits
            IValue edited = edits.Function(args).Var;
            IValue? delete = null;
            if(edited.Default != BasicTypes.NONE) {
                IValue? str = Values.ObjectLiteral.DeepGet(edited, "write", new List<IValue>()).Item1;
                delete = Values.ObjectLiteral.DeepGet(edited, "deleteFile", new List<IValue>()).Item1;
                IValue? append = Values.ObjectLiteral.DeepGet(edited, "append", new List<IValue>()).Item1;
                if(append != null && append.Var.Default != BasicTypes.NONE) {
                    file.Seek(0, SeekOrigin.End);
                    file.Write(Encoding.Unicode.GetBytes(append.String));
                } else if(str != null && str.Var.Default != BasicTypes.NONE) {
                    file.SetLength(0);
                    file.Write(Encoding.Unicode.GetBytes(str.String));
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
            return $"(edit file {GetArgument(0)})";
        }
    }
}