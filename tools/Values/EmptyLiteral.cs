namespace Tools.Values {
    class EmptyLiteral : IValue {
        private string Name { get; }
        public EmptyLiteral(string name) {
            this.Name = name;
        }
        public virtual BasicTypes Default {
            get {
                throw new RadishException("System error: empty literal accessed directly.");
            }
        }
        public virtual double Number {
            get {
                throw new RadishException($"Unable to parse {Name} as a number!");
            }
        }
        public virtual string String {
            get {
                throw new RadishException($"Unable to parse {Name} as a string!");
            }
        }
        public virtual bool Boolean {
            get {
                throw new RadishException($"Unable to parse {Name} as a boolean!");
            }
        }
        public virtual Dictionary<string, Variable> Object {
            get {
                throw new RadishException($"Unable to parse {Name} as an object!");
            }
        }

        public virtual IValue? Base { 
            get {
                return null;
            }
        }
        public virtual IValue Var {
            get {
                return this;
            }
            set {
                throw new RadishException($"Unable to parse {Name} as a variable!");
            }
        }
        public virtual Func<List<IValue>, IValue?, IValue?, IValue> Function {
            get {
                throw new RadishException($"Unable to parse {Name} as a function!");
            }
        }
        public virtual bool Equals(IValue other) {
            throw new RadishException($"Unable to check the equality of type {Name}!");
        }
        public virtual string Print() {
            throw new RadishException($"Unable to print a {Name} value!");
        }
        public virtual IValue? IsSuper {
            get {
                return null;
            }
            set {
                throw new RadishException($"Error: Attempted to use non-constructor variable as a constructor!");
            }
        }
    }
}