using System.Windows.Forms;
namespace Tools.Operators {
    class Output : Operator {
        protected IOperator Input { get; }
        public Output(IOperator input) : base(-1, -1) {
            this.Input = input;
        }

        protected string CalcOutput(Tools.IValue input, int spaces = 2) {
            if(input.Default == Tools.BasicTypes.OBJECT) {
                if(Values.ObjectLiteral.ArrayProto != null && input.Base == Values.ObjectLiteral.ArrayProto.Var) {
                    if(spaces >= 8) {
                        return "{Array}";
                    }
                    string returning = "[";
                    foreach(KeyValuePair<string, Values.Variable> val in input.Object) {
                        returning += CalcOutput(val.Value.Var, spaces + 2);
                        returning += ", ";
                    }
                    returning = returning.Substring(0, returning.Length - 2);
                    returning += "]";
                    return returning;
                } else {
                    if(spaces >= 8) {
                        return "{Object}";
                    }
                    //Console.WriteLine("passed array check");
                    string returning = "{\n";
                    foreach(KeyValuePair<string, Values.Variable> item in input.Object) {
                        returning += new System.String(' ', spaces);
                        returning += item.Key;
                        Tools.IValue? saved = item.Value.Host;
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
                return input.String;
            }
        }
        public override Tools.IValue Run(Stack Stack) {
            Tools.IValue result = Input._Run(Stack);
            //Console.WriteLine("about to access var");
            Console.WriteLine(CalcOutput(result.Var));
            //Console.WriteLine("done");

            return result;
        }
        public override string Print() {
            return $"(output {Input.Print()})";
        }
    }
}