namespace Tools.Operators {
    class EqualsEquals : SimpleOperator {
        public EqualsEquals(IOperator left, IOperator right) : base(left, right, "==") { }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.BooleanLiteral(leftResult.Number == rightResult.Number);
            }
            if(leftResult.Default == BasicTypes.STRING) {
                return new Values.BooleanLiteral(leftResult.String == rightResult.String);
            }
            if(leftResult.Default == BasicTypes.BOOLEAN) {
                return new Values.BooleanLiteral(leftResult.Boolean == rightResult.Boolean);
            }
            if(leftResult.Default == BasicTypes.ARRAY) {
                return new Values.BooleanLiteral(leftResult.Array == rightResult.Array);
            }
            if(leftResult.Default == BasicTypes.OBJECT) {
                return new Values.BooleanLiteral(leftResult.Object == rightResult.Object);
            }
            return new Values.BooleanLiteral(rightResult.Default == BasicTypes.NONE); // if left is null
        }
    }
}