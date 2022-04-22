namespace Tools.Operators {
    class Import : VariableOperator {
        private IOperator FileName { get; }
        public Import(Stack stack, IOperator fileName, int row, int col) : base(stack, row, col) {
            this.FileName = fileName;
        }
        public override IValue Run() {
            string name = FileName._Run().String;
            if(!name.EndsWith(".radish")) {
                name += ".radish";
            }
            CountingReader reader;
            try {
                reader = new CountingReader(name);
            } catch {
                throw new RadishException($"Could not find file {name}", Row, Col);
            }
            Operations operations = new Operations(reader, false);
            string previous = RadishException.FileName;
            RadishException.FileName = name;
            operations.ParseScope().Run();
            RadishException.FileName = previous;
            return new Values.ObjectLiteral(operations.stack.Pop().Val, Stack.Get("Object"));
        }
    }
}