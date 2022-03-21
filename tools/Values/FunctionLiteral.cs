namespace Tools.Values {
    class FunctionLiteral : IValue {
        private Stack Stack { get; }
        private List<string> ArgNames { get; }
        private IOperator Body { get; }
        public FunctionLiteral(Stack stack, List<string> argNames, IOperator body) {
            this.Stack = stack;
            this.ArgNames = argNames;
            this.Body = body;
        }
        public BasicTypes Default {
            get {
                return BasicTypes.FUNCTION;
            }
        }
        public double Number {
            get {
                throw new Exception("Function cannot be converted to number!");
            }
        }
        public string String {
            get {
                throw new Exception("Function cannot be converted to string!");
            }
        }
        public bool Boolean {
            get {
                return true;
            }
        }
        public List<IValue> Array {
            get {
                throw new Exception("Function cannot be converted to array!");
            }
        }
        public Dictionary<string, IValue> Object {
            get {
                throw new Exception("Function cannot be converted to object!");
            }
        }
        public IValue Var {
            get {
                throw new Exception("Function cannot be converted to variable!");
            }
            set {
                throw new Exception("Function cannot be converted to variable!");
            }
        }
        public void Function(List<IValue> args) {
            if(args.Count != ArgNames.Count) {
                throw new Exception("Invalid number of arguments!");
            }
            Stack.Push();
            for(int i = 0; i < args.Count; i++) {
                Stack.Head.Val.Add(new Variable(ArgNames[i], args[i]));
            }
            Body.Run();
            Stack.Pop();
        }
    }
}