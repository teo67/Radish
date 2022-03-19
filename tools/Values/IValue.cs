namespace Tools {
    interface IValue {
        public BasicTypes Default { get; }
        public double Number { get; }
        public string String { get; }
        public bool Boolean { get; }
        public Carrier Var { get; }
        public List<IValue> Array { get; }
        public Dictionary<string, IValue> Object { get; }
    }
}