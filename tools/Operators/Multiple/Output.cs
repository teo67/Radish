namespace Tools.Operators {
    class Output : IOperator {
        private IOperator Target { get; }
        public Output(IOperator target) {
            this.Target = target;
        }

        private string CalcOutput(IValue input, int spaces = 2) {
            if(input.Default == BasicTypes.ARRAY) {
                string returning = "[";
                for(int i = 0; i < input.Array.Count; i++) {
                    returning += CalcOutput(input.Array[i], spaces + 2);
                    if(i != input.Array.Count - 1) {
                        returning += ", ";
                    }
                }
                returning += "]";
                return returning;
            } else if(input.Default == BasicTypes.OBJECT) {
                string returning = "{\n";
                foreach(Values.Variable item in input.Object) {
                    if(item.Name == "base") {
                        continue;
                    }
                    returning += new System.String(' ', spaces);
                    returning += item.Name;
                    if(item.Default != BasicTypes.NONE) {
                        returning += ": ";
                        returning += CalcOutput(item.Var, spaces + 2);
                    }
                    returning += "\n";
                }
                returning += new System.String(' ', spaces - 2);
                returning += "}";
                return returning;
            } else if(input.Default == BasicTypes.FUNCTION) {
                return "Function";
            } else {
                return input.String;
            }
        }
        public IValue Run() {
            IValue result = Target.Run();
            Console.WriteLine(CalcOutput(result));
            return result;
        }
        public string Print() {
            return $"output({Target.Print()})";
        }
    }
}