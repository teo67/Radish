namespace Tools.Operators {
    class Execute  : SpecialOperator {
        public Execute(Librarian librarian) : base(librarian) {
        }
        public override IValue Run(Stack Stack) {
            Operations operations = new Operations(new StringReader(GetArgument(0)._Run(Stack).String), false, false, Librarian);
            string saved = RadishException.FileName;
            RadishException.FileName = "anonymous file";
            IValue returned = operations.ParseScope().Run(operations.stack);
            if(returned.Default == BasicTypes.RETURN) { 
                returned = returned.Function(new List<IValue>());
            }
            RadishException.FileName = saved;
            return returned;
        }
        public override string Print() {
            return $"execute ({GetArgument(0).Print()}))";
        }
    }
}