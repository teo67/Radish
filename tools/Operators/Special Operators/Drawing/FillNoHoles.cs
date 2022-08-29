namespace Tools.Operators {
    class FillNoHoles : BMPEditor {
        public FillNoHoles(Librarian librarian) : base("fillTriangle", 3, librarian) {}
        protected override void RunBMP(byte[] map, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {
            Dictionary<string, Values.Variable> zxs = GetArgument(1)._Run(Stack).Object;
            Dictionary<string, Values.Variable> zys = GetArgument(2)._Run(Stack).Object;
            if(zxs.Count != zys.Count) {
                throw new RadishException("The number of x coordinates supplied must be equal to the number of y coordinates supplied!");
            }
            int numPoints = zxs.Count;
            double[] xs = new double[numPoints];
            double[] ys = new double[numPoints];
            int z = 0;
            int miny = -1;
            int maxy = -1;
            foreach(KeyValuePair<string, Values.Variable> x in zxs) {
                xs[z] = x.Value.Number;
                z++;
            }
            z = 0;
            foreach(KeyValuePair<string, Values.Variable> y in zys) {
                ys[z] = y.Value.Number;
                if(miny == -1 || ys[z] < miny) {
                    miny = (int)Math.Floor(ys[z]);
                }
                if(maxy == -1 || ys[z] > maxy) {
                    maxy = (int)Math.Ceiling(ys[z]);
                }
                z++;
            }
            IValue color = GetArgument(3)._Run(Stack).Var;

            int?[] mins = new int?[maxy - miny + 1];
            int?[] maxs = new int?[maxy - miny + 1];

            for(int i = 0; i < numPoints; i++) {
                double x1 = xs[i];
                double y1 = ys[i];
                double x2 = xs[i == numPoints - 1 ? 0 : (i + 1)];
                double y2 = ys[i == numPoints - 1 ? 0 : (i + 1)];
                if(y1 == y2) {
                    continue;
                }
                double m = (x2 - x1) / (y2 - y1);
                int ry1 = (int)Math.Round(y1);
                int ry2 = (int)Math.Round(y2);
                int rx1 = (int)Math.Round(x1);
                int rx2 = (int)Math.Round(x2);
                int minr = Math.Min(ry1, ry2);
                int minrx = Math.Min(rx1, rx2);
                for(int j = minr; j <= (ry1 == minr ? ry2 : ry1); j++) {
                    int res = (int)Math.Round(m * (j - y1) + x1);
                    if(res < minrx) {
                        res = rx1;
                    } else if(res > (minrx == rx1 ? rx2 : rx1)) {
                        res = rx2;
                    }
                    if(mins[j - miny] == null || res < mins[j - miny]) {
                        mins[j - miny] = res;
                    }
                    if(maxs[j - miny] == null || res > maxs[j - miny]) {
                        maxs[j - miny] = res;
                    }
                }
            }
            

            for(int i = 0; i < mins.Length; i++) {
                int? min = mins[i];
                int? max = maxs[i];
                if(min != null && max != null) {
                    EditPixel(map, startIndex + rowLength * (height - i - miny - 1), (int)min, i + miny, width, bpp, color, (int)max - (int)min + 1);
                }
            }
        }
    }
}