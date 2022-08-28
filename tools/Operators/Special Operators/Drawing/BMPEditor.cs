namespace Tools.Operators {
    class BMPEditor  : SpecialOperator {
        private string Name { get; }
        private int NumArgs { get; }
        public BMPEditor(string name, int numArgs, Librarian librarian) : base(librarian) {
            this.Name = name;
            this.NumArgs = numArgs;
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
        protected void GetEquation(double x1, double x2, double y1, double y2, List<(int, int)> poses, Func<int, double>? xToY, Func<int, double>? yToX) {
            if(xToY != null) {
                for(int i = (int)Math.Round(x1); i <= (int)Math.Round(x2); i++) {
                    (int, int) res = (i, (int)Math.Round(xToY(i)));
                    if(!poses.Contains(res) && res.Item2 != -1) {
                        poses.Add(res);
                    }
                }
            }
            if(yToX != null) {
                for(int i = (int)Math.Round(y1); i <= (int)Math.Round(y2); i++) {
                    (int, int) res = ((int)Math.Round(yToX(i)), i);
                    if(!poses.Contains(res) && res.Item1 != -1) {
                        poses.Add(res);
                    } else {
                    }
                }
            }
        }
        protected void EditPixel(byte[] map, int rowStartIndex, int x, int width, int bpp, List<int> newValues, List<int>? numEach = null) {
            if(rowStartIndex >= map.Length || x >= width || rowStartIndex < 0 || x < 0) {
                throw new RadishException("Attempting to draw pixel out of range!");
            }
            int currentIndex = rowStartIndex + (int)Math.Floor((double)((x * bpp) / 8));
            int currentBit = 7 - ((x * bpp) % 8);
            byte current = map[currentIndex];
            for(int n = 0; n < newValues.Count; n++) {
                for(int m = 0; m < (numEach == null ? 1 : numEach[n]); m++) {
                    for(int i = 0; i < bpp; i++) {
                        if(currentBit < 0) {
                            currentBit = 7;
                            map[currentIndex] = current;
                            currentIndex++;
                            current = map[currentIndex];
                        }
                        byte exponent = (byte)(1 << currentBit);
                        bool setting = (newValues[n] & (1 << i)) != 0;
                        bool has = (current & exponent) != 0;
                        if(setting && !has) {
                            current += exponent;
                        } else if(has && !setting) {
                            current -= exponent;
                        }
                        currentBit--;
                    }
                }
            }
            if(currentBit < 7) {
                map[currentIndex] = current;
            }
        }
        public override IValue Run(Stack Stack) {
            string str = GetArgument(0)._Run(Stack).String;
            byte[] decoded = Convert.FromBase64String(str);
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
            string encoded = Convert.ToBase64String(decoded);//t.GetString(decoded, 0, decoded.Length);//System.Text.Encoding.Unicode.GetString(decoded, 0, decoded.Length);
            return new Values.StringLiteral(encoded);
        }
        public override string Print() {
            string returning = $"{Name}(";
            for(int i = 0; i < NumArgs; i++) {
                returning += GetArgument(i + 1).Print();
                if(i < NumArgs - 1) {
                    returning += ", ";
                }
            }
            return returning + ")";
        }
    }
}