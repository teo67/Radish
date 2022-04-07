// only difference from get is that this requires an operator not a string
namespace Tools.Operators {
    class BracketGet : VariableOperator {
        private IOperator Left { get; }
        private IOperator Name { get; }
        public BracketGet(IOperator left, IOperator name, Stack stack) : base(stack) {
            this.Left = left;
            this.Name = name;
        }
        public override IValue Run() {
            IValue result = Left.Run().Var;
            string nameR = Name.Run().String;
            IValue? gotten = Values.ObjectLiteral.Get(result, nameR, Stack, result);
            return new Values.PropertyHolder(gotten, nameR, result, Stack);
        }

        public override string Print() {
            return $"({Left.Print()}[{Name.Print()}])";
        }
    }
}