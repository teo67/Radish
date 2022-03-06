namespace Tools.Operators {
    class Add : SimpleOperator {
        public Add(IOperator left, IOperator right) : base(left, right, "+") { }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number + rightResult.Number);
            }
            if(leftResult.Default == BasicTypes.STRING) {
                return new Values.StringLiteral(leftResult.String + rightResult.String);
            }
            throw new Exception("The add operator only applies to numbers and strings!");
        }
    }
}