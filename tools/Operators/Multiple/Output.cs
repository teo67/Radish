namespace Tools.Operators {
    class Output : Operator {
        private IOperator Target { get; }
        public Output(IOperator target, int row, int col) : base(row, col) {
            this.Target = target;
        }

        private string CalcOutput(IValue input, int spaces = 2) {
            //Console.WriteLine("accessed");
            if(input.Default == BasicTypes.OBJECT) {
                //Console.WriteLine("obj");
                if(Values.ObjectLiteral.ValidateArray(input)) {
                    //Console.WriteLine("arr");
                    string returning = "[";
                    for(int i = 0; i < input.Object.Count; i++) {
                        returning += CalcOutput(input.Object[i].Var, spaces + 2);
                        if(i != input.Object.Count - 1) {
                            returning += ", ";
                        }
                    }
                    returning += "]";
                    return returning;
                } else {
                    //Console.WriteLine("passed array check");
                    string returning = "{\n";
                    foreach(Values.Variable item in input.Object) {
                        returning += new System.String(' ', spaces);
                        returning += item.Name;
                        IValue? saved = item.Host;
                        if(saved != null) {
                            returning += ": ";
                            returning += CalcOutput(saved, spaces + 2);
                        }
                        returning += "\n";
                    }
                    returning += new System.String(' ', spaces - 2);
                    returning += "}";
                    return returning;
                }
            } else if(input.Default == BasicTypes.FUNCTION) {
                return "{Function}";
            } else {
                //Console.WriteLine("accessing string");
                return input.String;
            }
        }
        public override IValue Run() {
            IValue result = Target._Run();
            //Console.WriteLine("about to access var");
            Console.WriteLine(CalcOutput(result.Var));
            //Console.WriteLine("done");
            return result;
        }
        public override string Print() {
            return $"holler({Target.Print()})";
        }
    }
}