namespace Tools.Operators {
    class IterateBMP  : BMPEditor {
        public IterateBMP(Librarian librarian) : base("iterateBMP", 1, librarian) {
        }
        protected override void RunBMP(byte[] bitmap, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {
            Func<List<IValue>, IValue> res = GetArgument(1)._Run(Stack).Function;
            for(int i = height - 1; i >= 0; i--) {
                int index = startIndex + (i * rowLength);
                for(int j = 0; j < width; j++) {
                    double received = (double)GetPixel(bitmap, index, j, bpp);
                    IValue result = res(new List<IValue>() { 
                        new Values.NumberLiteral(j),
                        new Values.NumberLiteral(height - 1 - i),
                        new Values.NumberLiteral((double)received)
                    }).Var;
                    if(result.Number == received) {
                        continue;
                    }
                    EditPixel(bitmap, index, j, -1, width, bpp, result);
                }
            }
        }
        public override string Print() {
            return "(iterate over bitmap)";
        }
    }
}