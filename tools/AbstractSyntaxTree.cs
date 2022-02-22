namespace Tools {
    class AbstractSyntaxTree {
        public List<AbstractSyntaxNode> allOperations;
        public AbstractSyntaxTree() {
            this.allOperations = new List<AbstractSyntaxNode>();
        }
        public static AbstractSyntaxTree GetTree(List<LexEntry> entries) {
            AbstractSyntaxTree tree = new AbstractSyntaxTree();
            Stack<AbstractSyntaxNode> currentStack = new Stack<AbstractSyntaxNode>();
            for(int i = 0; i < entries.Count; i++) {
                if(entries[i].Type == TokenTypes.ENDLINE) {
                    if(currentStack.Top != null) {
                        tree.allOperations.Add(currentStack.Top.Val);
                        currentStack = new Stack<AbstractSyntaxNode>();
                    }
                    continue;
                }
                int prevPrecedence = (currentStack.Top == null || currentStack.Top.Val.Val.Type != TokenTypes.OPERATOR) ? 100 : Operators.O[currentStack.Top.Val.Val.Val].Precedence;
                Operator? currentOperator = null;
                if(entries[i].Type == TokenTypes.OPERATOR) {
                    try {
                        currentOperator = Operators.O[entries[i].Val];
                    } catch {
                        throw new Exception($"Could not find operator {entries[i].Val}.");
                    }
                }
                int precedence = (currentOperator == null) ? 100 : currentOperator.Precedence; // 100 represents infinity in this case
                if(precedence <= prevPrecedence) {
                    
                } else {
                    
                }
            }
        }
    }
}