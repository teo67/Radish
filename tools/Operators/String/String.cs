namespace Tools.Operators {
    class String : Operator {
        private List<string> StringParts { get; }
        private List<IOperator> Interpolations { get; }
        public String(List<string> stringParts, List<IOperator> interpolations, int row, int col) : base(row, col) {
            this.StringParts = stringParts;
            this.Interpolations = interpolations;
        }
        public override IValue Run() {
            string returning = "";
            for(int i = 0; i < Interpolations.Count; i++) {
                returning += StringParts[i];
                returning += Interpolations[i]._Run().String;
            }
            returning += StringParts[StringParts.Count - 1];
            return new Values.StringLiteral(returning);
        }
        public override string Print() {
            string returning = "";
            for(int i = 0; i < Interpolations.Count; i++) {
                returning += StringParts[i];
                returning += " ... ";
            }
            returning += StringParts[StringParts.Count - 1];
            return returning;
        }
    }
}