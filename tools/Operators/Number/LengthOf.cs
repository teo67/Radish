namespace Tools.Operators {
    class LengthOf : Operator {
        private IOperator Arr { get; }
        public LengthOf(IOperator arr, int row, int col) : base(row, col) {
            this.Arr = arr;
        }
        public override IValue Run() {
            IValue returned = Arr._Run().Var;
            if(returned.Default == BasicTypes.OBJECT) {
                return new Values.NumberLiteral(returned.Object.Count);
            } else if(returned.Default == BasicTypes.STRING) {
                return new Values.NumberLiteral(returned.String.Length);
            } 
            throw new RadishException("The length property can only be applied to strings and arrays!");
        }
        public override string Print() {
            return $"(length of {Arr.Print()})";
        }
    }
}