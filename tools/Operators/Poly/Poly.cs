namespace Tools.Operators {
    class Poly : Operator {
        private List<IOperator> Params { get; }
        public Poly(List<IOperator> _params, int row, int col) : base(row, col) {
            Params = _params;
        }
        public override IValue Run(Stack Stack) {
            double? num = null;
            string? str = null;
            bool? boo = null;
            foreach(IOperator op in Params) {
                IValue res = op._Run(Stack).Var;
                switch(res.Default) {
                    case BasicTypes.NUMBER:
                        if(num != null) {
                            throw new RadishException("Poly-values can only store one number!", Row, Col);
                        }
                        num = res.Number;
                        break;
                    case BasicTypes.STRING:
                        if(str != null) {
                            throw new RadishException("Poly-values can only store one string!", Row, Col);
                        }
                        str = res.String;
                        break;
                    case BasicTypes.BOOLEAN:
                        if(boo != null) {
                            throw new RadishException("Poly-values can only store one boolean!", Row, Col);
                        }
                        boo = res.Boolean;
                        break;
                    default:
                        throw new RadishException("Poly-values can only store numbers, strings, and booleans!", Row, Col);
                }
            }
            return new Values.PolyLiteral(num == null ? 0.0 : (double)num, boo == null ? false : (bool)boo, str == null ? "" : (string)str);
        }
        public override string Print() {
            string returning = "(";
            for(int i = 0; i < Params.Count; i++) {
                returning += Params[i].Print();
                if(i != Params.Count - 1) {
                    returning += ", ";
                }
            }
            return returning + ")";
        }
    }
}