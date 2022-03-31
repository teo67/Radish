namespace Tools {
    class StackNode {
        public List<Values.Variable> Val { get; }
        public StackNode? Next { get; }
        public StackNode(List<Values.Variable> val, StackNode? next = null) {
            this.Val = val;
            this.Next = next;
        }
    }
    class Stack {
        public StackNode Head { get; private set; }
        public Stack(StackNode head) {
            this.Head = head;
        }
        public StackNode Push(List<Values.Variable>? head = null) {
            Head = new StackNode((head == null) ? new List<Values.Variable>() : head, Head);
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
                foreach(Values.Variable variable in viewing.Val) {
                    if(variable.Name == key) {
                        return variable;
                    }
                }
                viewing = viewing.Next;
            }
            throw new Exception($"Could not find variable {key}!");
        }
    }
}