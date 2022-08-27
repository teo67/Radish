namespace Tools.Operators {
    class BMPEditor  : SpecialOperator {
        public BMPEditor(Librarian librarian) : base(librarian) {
        }
        protected virtual void RunBMP(byte[] map, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {}
        protected int GetPixel(byte[] map, int rowStartIndex, int x, int bpp) {
            int currentIndex = rowStartIndex + (int)Math.Floor((double)((x * bpp) / 8));
            int currentBit = 7 - ((x * bpp) % 8);
            int current = 0;
            for(int i = 0; i < bpp; i++) {
                if(currentBit < 0) {
                    currentBit = 7;
                    currentIndex++;
                }
                byte exponent = (byte)(1 << currentBit);
                bool has = (current & exponent) != 0;
                if(has) {
                    current += (1 << i);
                }
                currentBit--;
            }
            return current;
        }
        protected void EditPixel(byte[] map, int rowStartIndex, int x, int bpp, int newValue) {
            int currentIndex = rowStartIndex + (int)Math.Floor((double)((x * bpp) / 8));
            int currentBit = 7 - ((x * bpp) % 8);
            byte current = map[currentIndex];
            for(int i = 0; i < bpp; i++) {
                if(currentBit < 0) {
                    currentBit = 7;
                    map[currentIndex] = current;
                    currentIndex++;
                    current = map[currentIndex];
                }
                byte exponent = (byte)(1 << currentBit);
                bool setting = (newValue & (1 << i)) != 0;
                bool has = (current & exponent) != 0;
                if(setting && !has) {
                    current += exponent;
                } else if(has && !setting) {
                    current -= exponent;
                }
                currentBit--;
            }
            if(currentBit < 7) {
                map[currentIndex] = current;
            }
        }
        public override IValue Run(Stack Stack) {
            string str = GetArgument(0)._Run(Stack).String;
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