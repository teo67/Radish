namespace Tools {
    class Operator { // where T is return type
        public List<TokenTypes> Args { get; }
        public int AfterNumArgs { get; }
        public Func<List<LexEntry>, LexEntry> Execute { get; }
        public int Precedence { get; } // precedence = -1 -> it always gets priority (kinda counterintuitive)
        public Operator(List<TokenTypes> args, Func<List<LexEntry>, LexEntry> execute, int afterNumArgs = 0, int precedence = 0) {
            this.Args = args;
            this.Execute = execute;
            this.AfterNumArgs = afterNumArgs;
            this.Precedence = precedence;
        }
    }
}