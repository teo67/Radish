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

            List<int>[] values = new List<int>[maxy - miny + 1];
            List<int>[] possibleHorizontalSkips = new List<int>[maxy - miny + 1];
            for(int i = 0; i < numPoints; i++) {
                double x1 = xs[i];
                double y1 = ys[i];
                double x2 = xs[i == numPoints - 1 ? 0 : (i + 1)];
                double y2 = ys[i == numPoints - 1 ? 0 : (i + 1)];
                
                int ry1 = (int)Math.Round(y1);
                int ry2 = (int)Math.Round(y2);
                int rx1 = (int)Math.Round(x1);
                int rx2 = (int)Math.Round(x2);
                int minr = Math.Min(ry1, ry2);
                int minrx = Math.Min(rx1, rx2);
                if(y1 == y2) {
                    InsertAtRightPlace(possibleHorizontalSkips, ry1 - miny, minrx);
                    EditPixel(map, startIndex + rowLength * (height - ry1 - 1), minrx, i + miny, width, bpp, color, Math.Abs(rx2 - rx1) + 1);
                    continue;
                }
                double m = (x2 - x1) / (y2 - y1);
                for(int j = minr; j <= (ry1 == minr ? ry2 : ry1); j++) {
                    int res = (int)Math.Round(m * (j - y1) + x1);
                    if(res < minrx) {
                        res = rx1;
                    } else if(res > (minrx == rx1 ? rx2 : rx1)) {
                        res = rx2;
                    }
                    InsertAtRightPlace(values, j - miny, res);
                }
            }
            

            for(int i = 0; i < values.Length; i++) {
                int skipIndex = 0;
                for(int j = 0; j < values[i].Count - 1; j += 2) {
                    EditPixel(map, startIndex + rowLength * (height - i - miny - 1), values[i][j], i + miny, width, bpp, color, values[i][j + 1] - values[i][j] + 1);
                    if(values[i].Count % 2 == 1 && skipIndex < possibleHorizontalSkips[i].Count) {
                        if(possibleHorizontalSkips[i][skipIndex] >= values[i][j] && possibleHorizontalSkips[i][skipIndex] <= values[i][j + 1]) {
                            EditPixel(map, startIndex + rowLength * (height - i - miny - 1), possibleHorizontalSkips[i][skipIndex], i + miny, width, bpp, color, values[i][j + 2] - possibleHorizontalSkips[i][skipIndex] + 1);
                            skipIndex++;
                            j++;
                        }
                    }
                }
            }
        }

        private void InsertAtRightPlace(List<int>[] arr, int i, int value) {
            if(arr[i] == null) {
                arr[i] = new List<int>();
            }
            int index = arr[i].Count;
            for(int k = 0; k < arr[i].Count; k++) {
                if(arr[i][k] >= value) {
                    index = k;
                            //Console.WriteLine($"I chose index {k} because {values[j - miny][k]} was more than {res} at y = {j - miny}");
                    break;
                }
            }
            arr[i].Insert(index, value);
        }
    }
}