namespace Tools.Operators {
    class Delete : Operator {
        private IOperator Left { get; }
        private string Name { get; }
        public Delete(IOperator left, string name, int row, int col) : base(row, col) {
            this.Left = left;
            this.Name = name;
        }
        public override IValue Run(Stack Stack) {
            Dictionary<string, Values.Variable> asVar = Left._Run(Stack).Object;
            Values.Variable? returning = null;
            asVar.TryGetValue(Name, out returning);
            bool deleted = asVar.Remove(Name);
            if(returning == null || !deleted) {
                throw new RadishException($"No property {Name} found to delete!", Row, Col);
            }
            return returning.Var;
        }

        public override string Print() {
            return $"({Left.Print()}:{Name})";
        }
    }
}