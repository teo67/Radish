namespace Tools.Operators {
    class ToJSON  : SpecialOperator {
        public ToJSON(Librarian librarian) : base(librarian) {
        }
        protected string CalcOutput(Tools.IValue input, int spaces, List<IValue> keepTrack) {
            if(keepTrack.Contains(input)) {
                throw new RadishException("Circular dependency detected!");
            }
            keepTrack.Add(input);
            string returning = "";
            if(input.Default == Tools.BasicTypes.OBJECT) {
                IValue? ct = Values.ObjectLiteral.CurrentThis;
                Values.ObjectLiteral.CurrentThis = input;
                if(Values.ObjectLiteral.ArrayProto != null && input.Base == Values.ObjectLiteral.ArrayProto.Var) {
                    returning = "[";
                    foreach(KeyValuePair<string, Values.Variable> val in input.Object) {
                        returning += CalcOutput(val.Value.Var, spaces, keepTrack);
                        returning += ", ";
                    }
                    if(returning.Length >= 2) {
                        returning = returning.Substring(0, returning.Length - 2);
                    }
                    returning += "]";
                    Values.ObjectLiteral.CurrentThis = ct;
                    return returning;
                } else {
                    //Console.WriteLine("passed array check");
                    returning = "{\n";
                    int count = 0;
                    foreach(KeyValuePair<string, Values.Variable> item in input.Object) {
                        returning += new System.String(' ', spaces);
                        returning += $"\"{item.Key}\"";
                        Tools.IValue? saved = item.Value.Host;
                        if(saved != null) {
                            returning += ": ";
                            returning += CalcOutput(saved, spaces + 4, keepTrack);
                        }
                        if(count != input.Object.Count - 1) {
                            returning += ",";
                        }
                        returning += "\n";
                        count++;
                    }
                    returning += new System.String(' ', spaces - 4);
                    returning += "}";
                    Values.ObjectLiteral.CurrentThis = ct;
                    return returning;
                }
            } else if(input.Default == Tools.BasicTypes.FUNCTION) {
                throw new RadishException("Unable to convert a tool into JSON format!");
            } else if(input.Default == BasicTypes.RETURN) {
                throw new RadishException("Unable to converted a value into JSON that is still being harvested!");
            } else if(input.Default == BasicTypes.STRING) {
                returning = $"\"{input.String}\"";
            } else if(input.Default == BasicTypes.BOOLEAN) {
                returning = input.Boolean ? "true" : "false";
            } else if(input.Default == BasicTypes.NUMBER) {
                returning = $"{input.Number}";
            } else if(input.Default == BasicTypes.NONE) {
                returning = "null";
            } else if(input.Default == BasicTypes.POLY) {
                throw new RadishException("JSON does not support poly-values!");
            }
            keepTrack.RemoveAt(keepTrack.Count - 1);
            return returning;
        }
        public override IValue Run(Stack Stack) {
            return new Values.StringLiteral(CalcOutput(GetArgument(0)._Run(Stack).Var, 4, new List<IValue>()));
        }
        public override string Print() {
            return $"({GetArgument(0).Print()} to json)";
        }
    }
}