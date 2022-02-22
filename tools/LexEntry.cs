namespace Tools {
    class LexEntry {
        public TokenTypes Type { get; }
        public string Val { get; }
        public LexEntry(TokenTypes type, string val) {
            this.Type = type;
            this.Val = val;
        }
    }
}