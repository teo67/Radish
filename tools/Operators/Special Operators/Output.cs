namespace Tools.Operators {
    class Output : Operator {
        protected IOperator Input { get; }
        public Output(IOperator input) : base(-1, -1) {
            this.Input = input;
        }

        protected string CalcOutput(Tools.IValue input, int spaces = 4) {
            if(input.Default == Tools.BasicTypes.OBJECT) {
                if(Values.ObjectLiteral.ArrayProto != null && input.Base == Values.ObjectLiteral.ArrayProto.Var) {
                    if(spaces >= 16) {
                        return "{Array}";
                    }
                    string returning = "[";
                    foreach(KeyValuePair<string, Values.Variable> val in input.Object) {
                        try {
                            IValue? previous = val.Value.ThisRef;
                            val.Value.ThisRef = input;
                            returning += CalcOutput(val.Value.Var, spaces);
                            val.Value.ThisRef = previous;
                            
                        } catch {
                            returning += "{unevaluated getter}";
                        }
                        returning += ", ";
                    }
                    returning = returning.Substring(0, returning.Length - 2);
                    returning += "]";
                    return returning;
                } else {
                    if(spaces >= 16) {
                        return "{Object}";
                    }
                    //Console.WriteLine("passed array check");
                    string returning = "{\n";
                    foreach(KeyValuePair<string, Values.Variable> item in input.Object) {
                        returning += new System.String(' ', spaces);
                        returning += item.Key;
                        try {
                            IValue? previous = item.Value.ThisRef;
                            item.Value.ThisRef = input;
                            Tools.IValue? saved = item.Value.Host;
                            item.Value.ThisRef = previous;
                            if(saved != null) {
                                returning += ": ";
                                returning += CalcOutput(saved, spaces + 4);
                            }
                        } catch {
                            returning += " { }";
                        }
                        returning += "\n";
                    }
                    returning += new System.String(' ', spaces - 4);
                    returning += "}";
                    return returning;
                }
            } else if(input.Default == Tools.BasicTypes.FUNCTION) {
                return "{Function}";
            } else {
                return input.String;
            }
        }
        public override Tools.IValue Run(Stack Stack) {
            //Console.WriteLine("before");
            Tools.IValue result = Input._Run(Stack);
            //Console.WriteLine("about to access var");
            //Console.WriteLine(result.Var.Default);
            Console.WriteLine(CalcOutput(result.Var));
            //Console.WriteLine("done");

            return result;
        }
        public override string Print() {
            return $"(output {Input.Print()})";
        }
    }
}