namespace Tools.Operators {
    class ArrayGet : SimpleOperator {
        public ArrayGet(IOperator left, IOperator right) : base(left, right, "array get") {}
        public override IValue Run() {
            double returned = Right.Run().Number;
            if(returned < 0) {
                throw new Exception("Could not index an array for a negative number!");
            }
            if(returned % 1 != 0) {
                throw new Exception("Array index must be an integer!");
            }
            return Left.Run().Array[(int)returned];
        }
    }
}