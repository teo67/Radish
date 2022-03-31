namespace Tools.Operators {
    class Assignment : SimpleOperator {
        private IValue Num { get; }
        private IValue Str { get; }
        public Assignment(IOperator left, IOperator right, IValue num, IValue str) : base(left, right, "=") {
            this.Num = num;
            this.Str = str;
        }
        public override IValue Run() {
            IValue result = Left.Run();
            IValue right = Right.Run();
            switch(right.Default) {
                case BasicTypes.NUMBER:
                    result.Var = new Values.NumberLiteral(right.Number, Num);
                    break;
                case BasicTypes.STRING:
                    result.Var = new Values.StringLiteral(right.String, Str);
                    break;
                case BasicTypes.BOOLEAN:
                    result.Var = new Values.BooleanLiteral(right.Boolean);
                    break;
                case BasicTypes.NONE:
                    result.Var = new Values.NoneLiteral();
                    break;
                default:
                    result.Var = right;
                    break;
            }
            return result;
        }
    }
}