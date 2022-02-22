namespace Tools {
    class AbstractSyntaxNode {
        public LexEntry Val { get; }
        public List<LexEntry> args;
        public AbstractSyntaxNode(LexEntry val, List<LexEntry>? args = null) {
            this.Val = val;
            this.args = (args == null) ? new List<LexEntry>() : args;
        }   
    }
}