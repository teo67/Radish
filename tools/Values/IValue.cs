namespace Tools {
    interface IValue {
        public BasicTypes Default { get; } // default type
        public double Number { get; } // get value as number
        public string String { get; } // get value as string
        public bool Boolean { get; } // get value as bool
        public IValue Var { get; set; } // get/set mutable variable inside of value
        public List<IValue> Array { get; } // get value as array 
        public Dictionary<string, IValue> Object { get; } // get value as object
        public void Function(List<IValue> args); // call value as function with list of values
    }
}