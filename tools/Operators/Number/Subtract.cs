namespace Tools.Operators {
    class Subtract : SimpleOperator {
        public Subtract(IOperator left, IOperator right) : base(left, right, "-") { }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number - rightResult.Number);
            }
            throw new Exception("The subtract operator only applies to numbers!");
        }
    }
}