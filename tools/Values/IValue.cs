namespace Tools {
    interface IValue {
        public BasicTypes Default { get; } // default type
        public double Number { get; } // get value as number
        public string String { get; } // get value as string
        public bool Boolean { get; } // get value as bool
        public IValue Var { get; set; } // get/set mutable variable inside of value
        public Dictionary<string, Values.Variable> Object { get; } // get value as object
        public IValue? Base { get; }
        public bool Equals(IValue other); // check equality
        public IOperator FunctionBody { get; } // get function in order to check for equality
        public IValue Function(List<IValue> args, IValue? _this); // call value as function with list of values
        public string Print();
        public IValue? IsSuper { get; set; }
    }
}