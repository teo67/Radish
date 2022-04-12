namespace Tools {
    interface IOperator {
        public IValue _Run();
        public string Print();
        public IValue OnError(RadishException error);
    }
}