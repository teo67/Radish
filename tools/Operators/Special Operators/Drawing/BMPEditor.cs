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
        protected void GetEquation(double x1, double x2, double y1, double y2, List<(int, int)> poses, Func<int, double>? xToY, Func<int, double>? yToX, bool ignoreDupes = false) {
            int rx1 = (int)Math.Round(x1);
            int rx2 = (int)Math.Round(x2);
            int ry1 = (int)Math.Round(y1);
            int ry2 = (int)Math.Round(y2);
            if(xToY != null) {
                for(int i = rx1; i <= rx2; i++) {
                    (int, int) res = (i, (int)Math.Round(xToY(i)));
                    if(res.Item2 != -1 || ignoreDupes) {
                        if(res.Item2 < ry1) {
                            res.Item2 = ry1;
                        } else if(res.Item2 > ry2) {
                            res.Item2 = ry2;
                        }
                        if(!poses.Contains(res) || ignoreDupes) {
                            poses.Add(res);
                        }
                    }
                }
            }
            if(yToX != null) {
                for(int i = (int)Math.Round(y1); i <= (int)Math.Round(y2); i++) {
                    (int, int) res = ((int)Math.Round(yToX(i)), i);
                    if(res.Item1 != -1 || ignoreDupes) {
                        if(res.Item1 < rx1) {
                            res.Item1 = rx1;
                        } else if(res.Item1 > rx2) {
                            res.Item1 = rx2;
                        }
                        if(!poses.Contains(res) || ignoreDupes) {
                            poses.Add(res);
                        }
                    }
                }
            }
        }
        protected void EditPixel(byte[] map, int rowStartIndex, int x, int y, int width, int bpp, IValue newValue, int numEach = 1) {
            if(rowStartIndex >= map.Length || x >= width || rowStartIndex < 0 || x < 0) {
                throw new RadishException("Attempting to draw pixel out of range!");
            }
            int currentIndex = rowStartIndex + (int)Math.Floor((double)((x * bpp) / 8));
            int currentBit = 7 - ((x * bpp) % 8);
            int solidColor = (newValue.Default == BasicTypes.NUMBER || newValue.Default == BasicTypes.POLY) ? (int)newValue.Number : -1;
            byte current = map[currentIndex];
                for(int m = 0; m < numEach; m++) {
                    int color = solidColor >= 0 ? solidColor : (int)Math.Round(newValue.Function(new List<IValue>() { new Values.NumberLiteral(m + x), new Values.NumberLiteral(y) }, null, null).Number);
                    for(int i = bpp - 1; i >= 0; i--) {
                        if(currentBit < 0) {
                            currentBit = 7;
                            map[currentIndex] = current;
                            currentIndex++;
                            current = map[currentIndex];
                        }
                        byte exponent = (byte)(1 << currentBit);
                        bool setting = (color & (1 << i)) != 0;
                        bool has = (current & exponent) != 0;
                        if(setting && !has) {
                            current += exponent;
                        } else if(has && !setting) {
                            current -= exponent;
                        }
                        currentBit--;
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