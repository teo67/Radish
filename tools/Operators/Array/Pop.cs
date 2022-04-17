namespace Tools.Operators {
    class Pop : SimpleOperator {
        public Pop(IOperator left, IOperator right, int row, int col) : base(left, right, "pop", row, col) {

        }
        public override IValue Run() {
            IValue arr = Left._Run().Var;
            int index = (int)(Right._Run().Number);
            if(index < 0) {
                throw new RadishException("Unable to remove from an array using an index less than zero!");
            }
            if(arr.Object.Count < index + 1) {
                throw new RadishException("Unable to remove from an array with an index greater than or equal to that array's size!");
            }
            if(index == arr.Object.Count - 1) {
                arr.Object.RemoveAt(index);
            } else {
                IValue last = arr.Object[arr.Object.Count - 1].Var;
                arr.Object.RemoveAt(arr.Object.Count - 1);
                for(int i = 0; i < arr.Object.Count; i++) {
                    if(arr.Object[i].Name != $"{i}") {
                        throw new RadishException("The remove() and pop() functions can only be called on arrays!");
                    }
                    if(i == arr.Object.Count - 1) {
                        arr.Object[i].Var = last;
                    } else if(i >= index) {
                        arr.Object[i].Var = arr.Object[i + 1].Var;
                    }
                }
            }
            return arr;
        }
    }
}