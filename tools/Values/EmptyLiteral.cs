namespace Tools.Values {
    class EmptyLiteral : IValue {
        private string Name { get; }
        public EmptyLiteral(string name) {
            this.Name = name;
        }
        public virtual BasicTypes Default {
            get {
                throw new Exception("System error: empty literal accessed directly.");
            }
        }
        public virtual double Number {
            get {
                throw new Exception($"Unable to parse {Name} as number!");
            }
        }
        public virtual string String {
            get {
                throw new Exception($"Unable to parse {Name} as string!");
            }
        }
        public virtual bool Boolean {
            get {
                throw new Exception($"Unable to parse {Name} as boolean!");
            }
        }
        public virtual List<IValue> Array {
            get {
                throw new Exception($"Unable to parse {Name} as array!");
            }
        }
        public virtual List<Variable> Object {
            get {
                throw new Exception($"Unable to parse {Name} as object!");
            }
        }
        public virtual IValue Var {
            get {
                throw new Exception($"Unable to parse {Name} as variable!");
            }
            set {
                throw new Exception($"Unable to parse {Name} as variable!");
            }
        }
        public virtual IValue Function(List<IValue> args) {
            throw new Exception($"Unable to parse {Name} as function!");
        }
    }
}