namespace Tools.Operators {
    class Output : IOperator {
        private IOperator Target { get; }
        public Output(IOperator target) {
            this.Target = target;
        }

        private string CalcOutput(IValue input) {
            if(input.Default == BasicTypes.ARRAY) {
                string returning = "[";
                for(int i = 0; i < input.Array.Count; i++) {
                    returning += CalcOutput(input.Array[i]);
                    if(i != input.Array.Count - 1) {
                        returning += ", ";
                    }
                }
                returning += "]";
                return returning;
            } else if(input.Default == BasicTypes.OBJECT) {
                string returning = "{";
                foreach(var item in input.Object) {
                    returning += item.Key;
                    returning += ": ";
                    returning += CalcOutput(item.Value);
                    returning += "\n";
                }
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