namespace Tools.Operators {
    class Delete : Operator {
        private IOperator Left { get; }
        private string Name { get; }
        public Delete(IOperator left, string name, int row, int col) : base(row, col) {
            this.Left = left;
            this.Name = name;
        }
        public override IValue Run() {
            List<Values.Variable> asVar = Left._Run().Object;
            IValue? returning = null;
            for(int i = 0; i < asVar.Count; i++) {
                if(asVar[i].Name == Name) {
                    returning = asVar[i];
                    asVar.RemoveAt(i);
                }
            }
            if(returning == null) {
                throw new RadishException($"No property {Name} found to delete!", Row, Col);
            }
            return returning;
        }

        public override string Print() {
            return $"({Left.Print()}:{Name})";
        }
    }
}