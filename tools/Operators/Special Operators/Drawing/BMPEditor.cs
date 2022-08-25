namespace Tools.Operators {
    class BMPEditor : Operator {
        protected IOperator Bitmap { get; }
        public BMPEditor(IOperator bitmap) : base(-1, -1) {
            Bitmap = bitmap;
        }
        protected virtual void RunBMP(byte[] map, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {}
        protected void EditPixel(int rowStartIndex, int x, int bpp, int newValue) {
            
        }
        public override IValue Run(Stack Stack) {
            string str = Bitmap._Run(Stack).String;
            byte[] decoded = System.Text.Encoding.Unicode.GetBytes(str, 0, str.Length);
            int width = 0;
            int height = 0;
            int offset = 0;
            int bpp = 0;
            for(int i = 0; i < 4; i++) {
                int multiplier = (1 << (i * 8));
                width += multiplier * decoded[i + 18];
                height += multiplier * decoded[i + 22];
                offset += multiplier * decoded[i + 10];
                if(i < 2) {
                    bpp += multiplier * decoded[i + 28];
                }
            }
            int rowLength = (int)Math.Ceiling(Math.Ceiling((bpp * width) / 8.0) / 4.0) * 4;
            RunBMP(decoded, offset, width, height, bpp, rowLength, Stack);
            string encoded = System.Text.Encoding.Unicode.GetString(decoded, 0, decoded.Length);
            return new Values.StringLiteral(encoded);
        }
        public override string Print() {
            return "(edit bmp)";
        }
    }
}