namespace Tools.Operators {
    class Execute : Operator {
        private Librarian Librarian { get; }
        private IOperator Text { get; }
        public Execute(IOperator text, Librarian librarian) : base(-1, -1) {
            this.Librarian = librarian;
            this.Text = text;
        }
        public override IValue Run(Stack Stack) {
            Operations operations = new Operations(new StringReader(Text._Run(Stack).String), false, false, Librarian);
            string saved = RadishException.FileName;
            RadishException.FileName = "anonymous file";
            IValue returned = operations.ParseScope().Run(operations.stack);
            if(returned.Default == BasicTypes.RETURN) { 
                returned = returned.Function(new List<IValue>(), null, null);
            }
            RadishException.FileName = saved;
            return returned;
        }
        public override string Print() {
            return $"execute ({Text.Print()}))";
        }
    }
}