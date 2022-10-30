namespace Tools {
    class Minifier : Operations {
        public string Output { get; private set; }
        private LexEntry Last { get; set; }
        public Minifier(IReader reader, Librarian librarian) : base(reader, false, false, librarian) {
            Output = "";
            Last = new LexEntry(TokenTypes.NONE, "");
        }
        private bool IsWord(LexEntry input) {
            return (input.Type == TokenTypes.KEYWORD || input.Type == TokenTypes.BOOLEAN || (input.Type == TokenTypes.OPERATOR && OpKeywords.Contains(input.Val)));
        }
        protected override LexEntry Read() {
            if(Stored != null) {
                LexEntry saved = Stored;
                Stored = null;
                return saved;
            }
            PrevCol = reader.col;
            PrevRow = reader.row;
            LexEntry ran = lexer.Run();
            if(
                (IsWord(Last) && IsWord(ran)) || (IsWord(Last) && ran.Type == TokenTypes.NUMBER) || (Last.Type == TokenTypes.OPERATOR && ran.Type == TokenTypes.OPERATOR)
            ) {
                Output += " ";
            }
            Output += ran.Val;
            Last = ran;
            return ran;
        }
        protected override void Handle(string input) {
            if(input == "harvest" || input == "plant" || input == "dig" || input == "tool" || input == "FUN") {
                Output = Output.Substring(0, Output.Length - input.Length + 1);
            }
        }
    }
}