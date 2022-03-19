namespace Tools {
    class StackNode {
        public Dictionary<string, Values.Variable> Val { get; }
        public StackNode? Next { get; }
        public StackNode(Dictionary<string, Values.Variable> val, StackNode? next = null) {
            this.Val = val;
            this.Next = next;
        }
    }
    class Stack {
        public StackNode Head { get; private set; }
        public Stack(StackNode head) {
            this.Head = head;
        }
        public StackNode Push(Dictionary<string, Values.Variable> val) {
            Head = new StackNode(val, Head);
            return Head;
        }
        public StackNode Pop() {
            StackNode saved = Head;
            if(Head.Next == null) {
                throw new Exception("Could not pop the last layer of the stack!");
            }
            Head = Head.Next;
            return saved;
        }
        public Values.Variable Get(string key) {
            StackNode? viewing = Head;
            while(viewing != null) {
                if(viewing.Val.ContainsKey(key)) {
                    return viewing.Val[key];
                }
                viewing = viewing.Next;
            }
            throw new Exception($"Could not find variable {key}!");
        }
    }
}