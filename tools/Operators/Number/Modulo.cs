namespace Tools.Operators {
    class Modulo : SimpleOperator {
        private IValue Num { get; }
        public Modulo(IOperator left, IOperator right, IValue num) : base(left, right, "%") {
            this.Num = num;
        }
        public override IValue Run() {
            IValue leftResult = Left.Run();
            IValue rightResult = Right.Run();
            if(leftResult.Default == BasicTypes.NUMBER) {
                return new Values.NumberLiteral(leftResult.Number % rightResult.Number, Num);
            }
            throw new Exception("The modulo operator only applies to numbers!");
        }
    }
}