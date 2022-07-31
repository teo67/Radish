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
        public StackNode Head { get; set; }
        private int Length { get; set; }
        public Stack(StackNode head) {
            this.Head = head;
            this.Length = 0;
        }
        public StackNode Push(Dictionary<string, Values.Variable>? head = null) {
            Length++;
            if(Length > 1000) { // BREAKPOINT 96
                throw new RadishException("Maximum stack size exceeded!");
            }
            Head = new StackNode((head == null) ? new Dictionary<string, Values.Variable>() : head, Head);
            return Head;
        }
        public StackNode Pop() {
            Length--;
            StackNode saved = Head;
            if(Head.Next == null) {
                throw new RadishException("Could not pop the last layer of the stack!");
            }
            Head = Head.Next;
            return saved;
        }
        public Values.Variable? SafeGet(string key) {
            StackNode? viewing = Head;
            while(viewing != null) {
                Values.Variable? variable = null;
                bool gotten = viewing.Val.TryGetValue(key, out variable);
                
                if(gotten && variable != null) {
                    return variable;
                }
                viewing = viewing.Next;
            }
            return null;
        }
        public void Print() {
            Console.WriteLine("__");
            StackNode? viewing = Head;
            while(viewing != null) {
                string returning = "[";
                foreach(KeyValuePair<string, Values.Variable> var in viewing.Val) {
                    returning += $"{var.Key}, ";
                }
                returning += "]";
                Console.WriteLine(returning);
                viewing = viewing.Next;
            }
            Console.WriteLine("__");
        }
    }
}