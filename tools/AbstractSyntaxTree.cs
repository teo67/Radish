namespace Tools {
    class AbstractSyntaxTree {
        private AbstractSyntaxNode? Head { get; }
        private Stack<Operator> opStack;
        private Stack<AbstractSyntaxNode> nodeStack;
        public AbstractSyntaxTree() {
            this.Head = null;
            this.opStack = new Stack<Operator>(
                new StackNode<Operator>(
                    new Operator(
                        Operators.O["Program"], 
                        new LexEntry(
                            TokenTypes.PROGRAM, "Program"
                        )
                    )
                )
            );
            this.nodeStack = new Stack<AbstractSyntaxNode>();
        }
        public void Add(LexEntry entry) {
            if(entry.Type == TokenTypes.KEYWORD) {
                
            }
        }
    }
}