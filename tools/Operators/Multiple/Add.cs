namespace Tools.Operators {
    class Add : SimpleOperator {
        private IValue Num { get; }
        private IValue Str { get; }
        public Add(IOperator left, IOperator right, IValue num, IValue str) : base(left, right, "+") {
            this.Num = num;
            this.Str = str;
        }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number + rightResult.Number, Num);
            }
            if(leftResult.Default == BasicTypes.STRING) {
                return new Values.StringLiteral(leftResult.String + rightResult.String, Str);
            }
            throw new Exception("The add operator only applies to numbers and strings!");
        }
    }
}