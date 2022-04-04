namespace Tools.Operators {
    class LengthOf : VariableOperator {
        private IOperator Arr { get; }
        public LengthOf(Stack stack, IOperator arr) : base(stack) {
            this.Arr = arr;
        }
        public override IValue Run() {
            IValue returned = Arr.Run();
            if(returned.Default == BasicTypes.OBJECT) {
                return new Values.NumberLiteral(returned.Object.Count, Stack.Get("Number").Var);
            } else if(returned.Default == BasicTypes.STRING) {
                return new Values.NumberLiteral(returned.String.Length, Stack.Get("Number").Var);
            } 
            throw new Exception("Lengthof can only be applied to strings and arrays!");
        }
        public override string Print() {
            return $"(length of {Arr.Print()})";
        }
    }
}