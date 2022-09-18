namespace Tools.Operators {
    class Output  : SpecialOperator {
        public Output(Librarian librarian) : base(librarian) {
        }

        protected string CalcOutput(Tools.IValue input, int spaces = 4) {
            if(input.Default == Tools.BasicTypes.OBJECT) {
                IValue? ct = Values.ObjectLiteral.CurrentThis;
                Values.ObjectLiteral.CurrentThis = ct;
                if(Values.ObjectLiteral.ArrayProto != null && input.Base == Values.ObjectLiteral.ArrayProto.Var) {
                    if(spaces >= 16) {
                        return "{Array}";
                    }
                    string returning = "[";
                    foreach(KeyValuePair<string, Values.Variable> val in input.Object) {
                        try {
                            returning += CalcOutput(val.Value.Var, spaces);
                        } catch {
                            returning += "{unevaluated getter}";
                        }
                        returning += ", ";
                    }
                    if(returning.Length >= 2) {
                        returning = returning.Substring(0, returning.Length - 2);
                    }
                    returning += "]";
                    Values.ObjectLiteral.CurrentThis = ct;
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
                            Tools.IValue? saved = item.Value.Host;
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
                    Values.ObjectLiteral.CurrentThis = ct;
                    return returning;
                }
            } else if(input.Default == Tools.BasicTypes.FUNCTION) {
                return "{Function}";
            } else if(input.Default == Tools.BasicTypes.POLY) {
                return $"({input.Number}, {(input.String == "" ? "\"\"" : input.String)}, {(input.Boolean ? "yes" : "no")})";
            } else {
                return input.String;
            }
        }
        public override Tools.IValue Run(Stack Stack) {
            //Console.WriteLine("before");
            Tools.IValue result = GetArgument(0)._Run(Stack);
            //Console.WriteLine("about to access var");
            //Console.WriteLine(result.Var.Default);
            Console.WriteLine(CalcOutput(result.Var));
            //Console.WriteLine("done");

            return result;
        }
        public override string Print() {
            return $"(output {GetArgument(0).Print()})";
        }
    }
}