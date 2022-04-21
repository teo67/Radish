namespace Tools.Values {
    class ValueWithScope : IValue {
        private IValue Scope { get; }
        private IValue Holding { get; }
        private Stack Stack { get; }
        public ValueWithScope(IValue scope, IValue holding, Stack stack) {
            this.Scope = scope;
            this.Holding = holding;
            this.Stack = stack;
        }
        public BasicTypes Default {
            get {
                return Holding.Default;
            }
        }
        public double Number {
            get {
                return Holding.Number;
            }
        }
        public string String {
            get {
                return Holding.String;
            }
        }
        public IOperator FunctionBody {
            get {
                return Holding.FunctionBody;
            }
        }
        public bool Boolean {
            get {
                return Holding.Boolean;
            }
        }
        public List<Variable> Object {
            get {
                return Holding.Object;
            }
        }
        public IValue? Base {
            get {
                return Holding.Base;
            }
        }
        public IValue Var {
            get {
                //return this;
                return Holding.Var;
            }
            set {
                Holding.Var = value;
            }
        }
        public bool Equals(IValue target) {
            return Holding.Equals(target);
        }
        public IValue Function(List<Variable> args) {
            Stack.Push(new List<Variable>() {
                new Variable("this", Scope)
            });
            IValue? saved = ObjectLiteral.CurrentPrivate;
            ObjectLiteral.CurrentPrivate = Holding;
            IValue returned = Holding.Function(args);
            Stack.Pop();
            ObjectLiteral.CurrentPrivate = saved;
            return returned;
        }
        public string Print() {
            return $"valuewithscope({Holding.Print()})"; // avoids infinite recursion lol
            //return $"valuewithscope(holding = ({Holding.Print()}), scope = ({Scope.Print()}))";
        }
    }
}