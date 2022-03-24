namespace Tools.Operators {
    class ReturnType : IOperator {
        private IOperator Stored { get; }
        private string Type { get; }
        public ReturnType(string type, IOperator stored) {
            string[] valid = { "out", "cancel", "continue" };
            if(!valid.Contains(type)) {
                throw new Exception($"{type} is not a valid return type!");
            }
            this.Type = type;
            this.Stored = stored;
        }
        public IValue Run() {
            return new Values.ReturnType(Type, Stored.Run());
        }
        public string Print() {
            return $"({Type} {Stored.Print()})";
        }
    }
}