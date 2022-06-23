namespace Tools.Values {
    class Character : StringLiteral {
        private IValue StringHolder { get; }
        private int CharIndex { get; }
        public Character(IValue stringHolder, int charIndex, string val) : base(val) {
            this.StringHolder = stringHolder;
            this.CharIndex = charIndex;
        }
        public override IValue Var { 
            get {
                return new StringLiteral(String);
            }
            set {
                char[] arr = StringHolder.String.ToCharArray();
                arr[CharIndex] = value.String[0];
                StringHolder.Var =  new StringLiteral(new string(arr));
            }
        }
    }
}