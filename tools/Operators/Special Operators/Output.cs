namespace Tools.Operators {
    class Output : VariableOperator {
        private IOperator Input { get; }
        public Output(IOperator input, Stack stack) : base(stack, -1, -1) {
            this.Input = input;
        }

        private string CalcOutput(Tools.IValue input, int spaces = 2) {
            //Console.WriteLine("accessed");
            if(input.Default == Tools.BasicTypes.OBJECT) {
                //Console.WriteLine("obj");
                if(Tools.Values.ObjectLiteral.ValidateArray(input)) {
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
                    foreach(Tools.Values.Variable item in input.Object) {
                        returning += new System.String(' ', spaces);
                        returning += item.Name;
                        Tools.IValue? saved = item.Host;
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
            } else if(input.Default == Tools.BasicTypes.FUNCTION) {
                return "{Function}";
            } else {
                //Console.WriteLine("accessing string");
                return input.String;
            }
        }
        public override Tools.IValue Run() {
            Tools.IValue result = Input._Run();
            //Console.WriteLine("about to access var");
            Console.WriteLine(CalcOutput(result.Var));
            //Console.WriteLine("done");
            return result;
        }
    }
}