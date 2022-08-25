namespace Tools.Operators {
    class IterateBMP : BMPEditor {
        private IOperator Func { get; }
        public IterateBMP(IOperator text, IOperator func) : base(text) {
            Func = func;
        }
        protected override void RunBMP(byte[] bitmap, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {
            Func<List<IValue>, IValue?, IValue?, IValue> res = Func._Run(Stack).Function;
            for(int i = height - 1; i >= 0; i--) {
                int index = startIndex + (i * rowLength);
                for(int j = 0; j < width; j++) {
                    int result = (int)res(new List<IValue>() { 
                        new Values.NumberLiteral(j),
                        new Values.NumberLiteral(height - 1 - i),
                    }, null, null).Number;
                }
            }
        }
    }
}