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
        public StackNode Head { get; set; }
        private int Length { get; set; }
        public Stack(StackNode head) {
            this.Head = head;
            this.Length = 0;
        }
        public StackNode Push(List<Values.Variable>? head = null) {
            Length++;
            if(Length > 1000) { // BREAKPOINT 96
                throw new RadishException("Maximum stack size of 1470 exceeded!");
            }
            Head = new StackNode((head == null) ? new List<Values.Variable>() : head, Head);
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
                foreach(Values.Variable variable in viewing.Val) {
                    if(variable.Name == key && variable.ProtectionLevel == ProtectionLevels.PUBLIC) {
                        return variable;
                    }
                }
                viewing = viewing.Next;
            }
            return null;
        }
        public Values.Variable Get(string key) {
            Values.Variable? returned = SafeGet(key);
            if(returned == null) {
                throw new RadishException($"Unable to find variable {key}!");
            }
            return returned;
        }
        public void Print() {
            Console.WriteLine("__");
            StackNode? viewing = Head;
            while(viewing != null) {
                string returning = "[";
                foreach(Values.Variable var in viewing.Val) {
                    returning += $"{var.Name}, ";
                }
                returning += "]";
                Console.WriteLine(returning);
                viewing = viewing.Next;
            }
            Console.WriteLine("__");
        }
    }
}