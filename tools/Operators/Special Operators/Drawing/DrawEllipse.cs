namespace Tools.Operators {
    class DrawEllipse : BMPEditor {
        public DrawEllipse(Librarian librarian) : base("drawEllipse", 6, librarian) {}
        protected override void RunBMP(byte[] map, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {
            double x = GetArgument(1)._Run(Stack).Number;
            double y = GetArgument(2)._Run(Stack).Number;
            double w = GetArgument(3)._Run(Stack).Number;
            double h = GetArgument(4)._Run(Stack).Number;
            int color = (int)GetArgument(5)._Run(Stack).Number;
            bool fill = GetArgument(6)._Run(Stack).Boolean;
            List<(int, int)> poses = new List<(int, int)>();
            GetEquation(x, x + w/2, y, y + h/2, poses, (fill ? null : (int cx) => {
                double sqrter = 1 - Math.Pow(cx - x, 2.0)/Math.Pow((w/2), 2.0);
                return sqrter < 0 ? y : ((h/2) * Math.Sqrt(sqrter)) + y;
            }), (int cy) => {
                double sqrter = 1 - Math.Pow(cy - y, 2.0)/Math.Pow((h/2), 2.0);
                if(sqrter < 0) {
                    Console.WriteLine(cy);
                }
                return sqrter < 0 ? x : ((w/2) * Math.Sqrt(sqrter)) + x;
            });
            if(fill) {
                foreach((int, int) pose in poses) {
                    Console.WriteLine($"{pose.Item1}, {pose.Item2}");
                    int left = (int)Math.Round(2 * x - pose.Item1);
                    int wi = (int)Math.Round(2 * (pose.Item1 - x)) + 1;
                    EditPixel(map, startIndex + (height - pose.Item2 - 1) * rowLength, left, width, bpp, new List<int>() { color }, new List<int>() { wi } );
                    EditPixel(map, startIndex + (height - (int)Math.Round(2 * y - pose.Item2) - 1) * rowLength, left, width, bpp, new List<int>() { color }, new List<int>() { wi } );
                }
            } else {
                foreach((int, int) pos in poses) {
                    EditPixel(map, startIndex + (height - pos.Item2 - 1) * rowLength, pos.Item1, width, bpp, new List<int>() { color });
                    EditPixel(map, startIndex + (height - pos.Item2 - 1) * rowLength, (int)Math.Round(2 * x - pos.Item1), width, bpp, new List<int>() { color });
                    EditPixel(map, startIndex + (height - (int)Math.Round(2 * y - pos.Item2) - 1) * rowLength, pos.Item1, width, bpp, new List<int>() { color });
                    EditPixel(map, startIndex + (height - (int)Math.Round(2 * y - pos.Item2) - 1) * rowLength, (int)Math.Round(2 * x - pos.Item1), width, bpp, new List<int>() { color });
                }
            }
        }
    }
}