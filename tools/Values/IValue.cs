namespace Tools {
    interface IValue {
        public BasicTypes Default { get; } // default type
        public double Number { get; } // get value as number
        public string String { get; } // get value as string
        public bool Boolean { get; } // get value as bool
        public IValue Var { get; set; } // get/set mutable variable inside of value
        public Dictionary<string, Values.Variable> Object { get; } // get value as object
        public IValue? Base { get; set; }
        public bool Equals(IValue other); // check equality
        public Func<List<IValue>, IValue?, IValue?, IValue> Function { get; } // call value as function with list of values
        public string Print();
    }
}