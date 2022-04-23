namespace Tools.Operators {
    class Import : VariableOperator {
        private IOperator FileName { get; }
        public Import(Stack stack, IOperator fileName, int row, int col) : base(stack, row, col) {
            this.FileName = fileName;
        }
        public override IValue Run() {
            string name = FileName._Run().String;
            if(!name.EndsWith(".rdsh")) {
                name += ".rdsh";
            }
            CountingReader reader;
            try {
                reader = new CountingReader(name);
            } catch {
                throw new RadishException($"Could not find file {name}", Row, Col);
            }
            Operations operations = new Operations(reader, false);
            RadishException.Append($"in {name}", -1, -1);
            IValue returned = operations.ParseScope().Run();
            RadishException.Pop();
            return (returned.Default == BasicTypes.RETURN ? returned.Function(new List<Values.Variable>()) : returned);
        }
    }
}