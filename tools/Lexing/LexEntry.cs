namespace Tools {
    class LexEntry {
        public TokenTypes Type { get; }
        public string Val { get; }
        public string? Raw { get; }
        public LexEntry(TokenTypes type, string val, string raw) {
            this.Type = type;
            this.Val = val;
            this.Raw = (raw == val) ? null : raw;
        }
    }
}