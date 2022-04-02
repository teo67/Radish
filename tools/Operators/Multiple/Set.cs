namespace Tools.Operators {
    class Set : SimpleOperator {
        private IOperator Middle { get; }
        public Set(IOperator left, IOperator middle, IOperator right) : base(left, right, "set") { // as in object.property = value
            this.Middle = middle;
        }
        public override IValue Run() {
            IValue result = Left.Run();
            string middle = Middle.Run().String;
            Values.Variable? found = null;
            foreach(Values.Variable property in result.Object) {
                if(property.Name == middle) {
                    found = property;
                    break;
                }
            }
            if(found == null) {
                Values.Variable returning = new Values.Variable(middle, Right.Run());
                result.Object.Add(returning);
                return returning;
            }
            found.Var = Right.Run();
            return found;
        }
    }
}