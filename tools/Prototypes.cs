namespace Tools { // adds basic prototypes to call stack
    class Prototypes {
        public static Dictionary<string, IValue> Imports { get; }
        public static List<Values.Variable> FirstLayer { get; }
        static Prototypes() {
            Imports = new Dictionary<string, IValue>();
            FirstLayer = new List<Values.Variable>();
            IValue Object = new Values.ObjectLiteral(new List<Values.Variable>(), null);
            FirstLayer.Add(new Values.Variable("Object", Object));
            IValue Number = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            FirstLayer.Add(new Values.Variable("Number", Number));
            IValue String = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            FirstLayer.Add(new Values.Variable("String", String));
            IValue Array = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            FirstLayer.Add(new Values.Variable("Array", Array));
            IValue Boolean = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            FirstLayer.Add(new Values.Variable("Boolean", Boolean));
            IValue Function = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            FirstLayer.Add(new Values.Variable("Function", Function));
        }
    }
}