namespace Tools.Operators {
    class DefineProperty : SpecialOperator {
        public DefineProperty(Librarian librarian) : base(librarian){}
        public override IValue Run(Stack Stack) {
            IValue obj = GetArgument(0)._Run(Stack).Var;
            string varName = GetArgument(1)._Run(Stack).String;
            IValue plant = GetArgument(2)._Run(Stack).Var;
            IValue harvest = GetArgument(3)._Run(Stack).Var;
            obj.Object[varName] = new Values.Property(harvest.Default == BasicTypes.NONE ? null : harvest, plant.Default == BasicTypes.NONE ? null : plant);
            return obj;
        }
        public override string Print() {
            return $"defineProperty({GetArgument(0).Print()}, {GetArgument(1).Print()})";
        }
    }
}