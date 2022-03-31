namespace Tools.Operators {
    class Divide : SimpleOperator {
        private IValue Num { get; }
        public Divide(IOperator left, IOperator right, IValue num) : base(left, right, "/") {
            this.Num = num;
        }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number / rightResult.Number, Num);
            }
            throw new Exception("The divide operator only applies to numbers!");
        }
    }
}