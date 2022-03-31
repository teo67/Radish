namespace Tools.Operators {
    class String : IOperator {
        private string Stored { get; }
        private IValue Str { get; }
        public String(string stored, IValue str) {
            this.Stored = stored;
            this.Str = str;
        }
        public IValue Run() {
            return new Values.StringLiteral(Stored, Str);
        }
        public string Print() {
            return $"\"{Stored}\"";
        }
    }
}