namespace Tools.Values {
    class ThisFunction : IValue {
        private IValue This { get; }
        private IValue _Function { get; }
        private Stack Stack { get; }
        public ThisFunction(IValue _this, IValue function, Stack stack) {
            this.Stack = stack;
            this.This = _this;
            this._Function = function;
            if(function.Default != BasicTypes.FUNCTION) {
                throw new Exception("This function not encapsulating a function!");
            }
        }
        public BasicTypes Default {
            get {
                return _Function.Default;
            }
        }
        public double Number {
            get {
                return _Function.Number;
            }
        }
        public string String {
            get {
                return _Function.String;
            }
        }
        public bool Boolean {
            get {
                return _Function.Boolean;
            }
        }
        public List<IValue> Array {
            get {
                return _Function.Array;
            }
        }
        public List<Variable> Object {
            get {
                return _Function.Object;
            }
        }
        public IValue Var {
            get {
                return _Function.Var;
            }
            set {
                _Function.Var = value;
            }
        }
        public IOperator FunctionBody {
            get {
                return _Function.FunctionBody;
            }
        }
        public IValue Clone() {
            return new ThisFunction(This, _Function, Stack);
        }
        public bool Equals(IValue other) {
            return _Function.Equals(other);
        }
        public IValue Function(List<IValue> args) {
            Stack.Push(new List<Variable>() {
                new Variable("this", This)
            });
            IValue returned = _Function.Function(args);
            Stack.Pop();
            return returned;
        }
    }
}