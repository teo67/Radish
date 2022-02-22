namespace Tools {
    class StackNode<T> {
        public T Val { get; }
        public StackNode<T>? next;
        public StackNode(T val, StackNode<T>? next = null) {
            this.Val = val;
            this.next = next;
        }
    }
    class Stack<T> {
        public StackNode<T>? Top { get; private set; }
        public Stack(StackNode<T>? top = null) {
            this.Top = top;
        }
        public void Push(T val) {
            this.Top = new StackNode<T>(val, this.Top);
        }
        public T Pop() {
            if(this.Top == null) {
                throw new Exception("Cannot pop an empty stack!");
            }
            StackNode<T> saved = this.Top;
            this.Top = this.Top.next;
            return saved.Val;
        }
    }
}