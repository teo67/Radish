namespace Tools.Operators {
    class LengthOf : IOperator {
        private IOperator Arr { get; }
        private IValue Num { get; }
        public LengthOf(IOperator arr, IValue num) {
            this.Arr = arr;
            this.Num = num;
        }
        public IValue Run() {
            IValue returned = Arr.Run();
            if(returned.Default == BasicTypes.ARRAY) {
                return new Values.NumberLiteral(returned.Array.Count, Num);
            } else if(returned.Default == BasicTypes.STRING) {
                return new Values.NumberLiteral(returned.String.Length, Num);
            } 
            throw new Exception("Lengthof can only be applied to strings and arrays!");
        }
        public string Print() {
            return $"(length of {Arr.Print()})";
        }
    }
}