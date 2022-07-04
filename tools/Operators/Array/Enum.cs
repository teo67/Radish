namespace Tools.Operators {
    class Enum : Operator {
        private List<string> List { get; }
        public Enum(List<string> List, int row, int col) : base(row, col) {
            this.List = List;
        }
        public override IValue Run(Stack Stack) {
            List<Values.Variable> num = new List<Values.Variable>();
            for(int i = 0; i < List.Count; i++) {
                num.Add(new Values.Variable(List[i], new Values.NumberLiteral(i)));
            }
            return new Values.ObjectLiteral(num, useProto: true);
        }
        public override string Print() {
            string returning = "enum {";
            foreach(string str in List) {
                returning += $"{str}, ";
            }
            returning += "}";
            return returning;
        }
    }
}