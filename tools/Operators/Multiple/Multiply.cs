namespace Tools.Operators {
    class Multiply : SimpleOperator {
        private IValue Num { get; }
        private IValue Str { get; }
        public Multiply(IOperator left, IOperator right, IValue num, IValue str) : base(left, right, "*") {
            this.Num = num;
            this.Str = str;
        }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number * rightResult.Number, Num);
            }
            if(leftResult.Default == BasicTypes.STRING) {
                if(rightResult.Default == BasicTypes.NUMBER) {
                    string adding = "";
                    for(int i = 0; i < rightResult.Number; i++) {
                        adding += leftResult.String;
                    }
                    return new Values.StringLiteral(adding, Str);
                }
                throw new Exception("Can only multiply a string by a number!");
            }
            throw new Exception("The multiply operator only applies to numbers and strings!");
        }
    }
}