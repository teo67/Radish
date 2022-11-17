using System.Reflection;
namespace Tools.Operators {
    enum DLLTypes {
        STRING, DOUBLE, INTEGER, BOOLEAN, STRING_ARRAY, DOUBLE_ARRAY, INTEGER_ARRAY, BOOLEAN_ARRAY
    }
    class ImportDLL : SpecialOperator {
        public ImportDLL(Librarian librarian) : base(librarian) {
        }
        private IValue GetValue(DLLTypes type, object? returned) {
            if(returned == null) {
                return new Values.NoneLiteral();
            }
            if(type == DLLTypes.STRING) {
                return new Values.StringLiteral((string)returned);
            }
            if(type == DLLTypes.DOUBLE) {
                return new Values.NumberLiteral((double)returned);
            }
            if(type == DLLTypes.INTEGER) {
                return new Values.NumberLiteral((int)returned);
            }
            if(type == DLLTypes.BOOLEAN) {
                return new Values.BooleanLiteral((bool)returned);
            }
            Dictionary<string, Values.Variable> dict = new Dictionary<string, Values.Variable>();
            if(type == DLLTypes.STRING_ARRAY) {
                string[] arr = (string[])(returned);
                for(int i = 0; i < arr.Length; i++) {
                    dict.Add($"{i}", new Values.Variable(GetValue(DLLTypes.STRING, arr[i])));
                }
            } else if(type == DLLTypes.INTEGER_ARRAY) {
                int[] arr = (int[])(returned);
                for(int i = 0; i < arr.Length; i++) {
                    dict.Add($"{i}", new Values.Variable(GetValue(DLLTypes.INTEGER, arr[i])));
                }
            } else if(type == DLLTypes.DOUBLE_ARRAY) {
                double[] arr = (double[])(returned);
                for(int i = 0; i < arr.Length; i++) {
                    dict.Add($"{i}", new Values.Variable(GetValue(DLLTypes.DOUBLE, arr[i])));
                }
            } else if(type == DLLTypes.BOOLEAN_ARRAY) {
                bool[] arr = (bool[])(returned);
                for(int i = 0; i < arr.Length; i++) {
                    dict.Add($"{i}", new Values.Variable(GetValue(DLLTypes.BOOLEAN, arr[i])));
                }
            } else {
                throw new Exception();
            }
            return new Values.ObjectLiteral(dict, useArrayProto: true);
        }
        private object? GetObject(IValue basic) {
            Dictionary<string, Values.Variable> resolved = basic.Object;
            Values.Variable? _val = null;
            Values.Variable? _type = null;
            bool worked1 = resolved.TryGetValue("value", out _val);
            bool worked2 = resolved.TryGetValue("valueType", out _type);
            if(_val == null || _type == null || !worked1 || !worked2) {
                throw new Exception();
            }
            IValue val = _val.Var;
            IValue type = _type.Var;
            DLLTypes realType = (DLLTypes)((int)type.Number);
            if(realType == DLLTypes.STRING) {
                return val.String;
            } if(realType == DLLTypes.DOUBLE) {
                return val.Number;
            } if(realType == DLLTypes.INTEGER) {
                return (int)Math.Round(val.Number);
            } if(realType == DLLTypes.BOOLEAN) {
                return val.Boolean;
            }
            Dictionary<string, Values.Variable> valObject = val.Object;
            if(realType == DLLTypes.STRING_ARRAY) { // i know this code is repetitive there is literally no other way to do it while preserving type
                string[] arr = new string[valObject.Count];
                int i = 0;
                foreach(KeyValuePair<string, Values.Variable> pair in valObject) {
                    arr[i] = pair.Value.String;
                    i++;
                }
                return arr;
            }
            if(realType == DLLTypes.INTEGER_ARRAY) {
                int[] arr = new int[valObject.Count];
                int i = 0;
                foreach(KeyValuePair<string, Values.Variable> pair in valObject) {
                    arr[i] = (int)Math.Round(pair.Value.Number);
                    i++;
                }
                return arr;
            }
            if(realType == DLLTypes.DOUBLE_ARRAY) {
                double[] arr = new double[valObject.Count];
                int i = 0;
                foreach(KeyValuePair<string, Values.Variable> pair in valObject) {
                    arr[i] = pair.Value.Number;
                    i++;
                }
                return arr;
            }
            if(realType == DLLTypes.BOOLEAN_ARRAY) {
                bool[] arr = new bool[valObject.Count];
                int i = 0;
                foreach(KeyValuePair<string, Values.Variable> pair in valObject) {
                    arr[i] = pair.Value.Boolean;
                    i++;
                }
                return arr;
            }
            throw new Exception();
        }
        public override IValue Run(Stack Stack) {
            string pathToFile = GetArgument(0)._Run(Stack).String;
            string typeName = GetArgument(1)._Run(Stack).String;
            Dictionary<string, Values.Variable> method = GetArgument(2)._Run(Stack).Object;
            System.Type? type;
            try {
                Assembly assembly = Assembly.LoadFile(pathToFile);
                type = assembly.GetType(typeName);
                if(type == null) {
                    throw new Exception();
                }
            } catch {
                throw new RadishException($"There was an error loading {pathToFile} and {typeName}!");
            }
            object? instance;
            try {
                instance = Activator.CreateInstance(type);
                if(instance == null) {
                    throw new Exception();
                }
            } catch {
                throw new RadishException($"There was an error creating a new object of type {typeName}.", Row, Col);
            }
            Values.Variable? args = null;
            Values.Variable? name = null;
            Values.Variable? returnVal = null;
            method.TryGetValue("arguments", out args);
            method.TryGetValue("name", out name);
            method.TryGetValue("return", out returnVal);
            if(name == null) {
                throw new RadishException($"At least one method in {typeName} is missing a name!", Row, Col);
            }
            string methodName = name.String;
            MethodInfo? _method;
            try {
                _method = type.GetMethod(methodName);
                if(_method == null) {
                    throw new Exception();
                }
            } catch {
                throw new RadishException($"There was an error loading the {methodName} method in {typeName}!", Row, Col);
            }
            object?[]? realArgs = null;
            if(args != null) {
                IValue __args = args.Var;
                if(__args.Default != BasicTypes.NONE) {
                    Dictionary<string, Values.Variable> _args = __args.Object;
                    realArgs = new object?[_args.Count];
                    int index = 0;
                    foreach(KeyValuePair<string, Values.Variable> arg in _args) {
                        IValue realArg = arg.Value.Var;
                        object? adding;
                        try {
                            adding = GetObject(realArg); 
                        } catch(RadishException) {
                            throw;
                        } catch {
                            throw new RadishException($"There was an error retrieving an argument for the {methodName} method in {typeName}.", Row, Col);
                        }
                        realArgs[index] = adding;
                        index++;
                    }
                }
            }
            object? result = _method.Invoke(instance, realArgs);
            if(returnVal != null) {
                IValue _returnVal = returnVal.Var;
                if(_returnVal.Default != BasicTypes.NONE) {
                    DLLTypes returnType = (DLLTypes)((int)_returnVal.Number);
                    try {
                        return GetValue(returnType, result);
                    } catch {
                        throw new RadishException($"There was an error getting the return value for {methodName} in {typeName}.", Row, Col);
                    }
                }
            }
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return $"importDLL({GetArgument(0).Print()})";
        }
    }
}