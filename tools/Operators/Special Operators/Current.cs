namespace Tools.Operators {
    enum DateType {
        YEAR, MONTH, DAY, HOUR, MINUTE, SECOND, MILLISECOND
    }
    class Current  : SpecialOperator {
        private DateType Type { get; }
        public Current(DateType type, Librarian librarian) : base(librarian) {
            this.Type = type;
        }
        public override IValue Run(Stack Stack) {
            double res = 0;
            DateTime now = new DateTime(1970, 1, 1).AddMilliseconds(GetArgument(0)._Run(Stack).Number);
            switch(Type) {
                case DateType.YEAR:
                    res = now.Year;
                    break;
                case DateType.MONTH:
                    res = now.Month - 1; // 0-index :)
                    break;
                case DateType.DAY:
                    res = now.Day - 1; // also 0-index
                    break;
                case DateType.HOUR:
                    res = now.Hour;
                    break;
                case DateType.MINUTE:
                    res = now.Minute;
                    break;
                case DateType.SECOND:
                    res = now.Second;
                    break;
                case DateType.MILLISECOND:
                    res = now.Millisecond;
                    break;
            }
            return new Values.NumberLiteral(res);
        }
        public override string Print() {
            return $"(gettime {Type})";
        }
    }
}