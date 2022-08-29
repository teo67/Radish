namespace Tools.Operators {
    class DrawEllipse : BMPEditor {
        public DrawEllipse(Librarian librarian) : base("drawEllipse", 6, librarian) {}
        protected override void RunBMP(byte[] map, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {
            double x = GetArgument(1)._Run(Stack).Number;
            double y = GetArgument(2)._Run(Stack).Number;
            double w = GetArgument(3)._Run(Stack).Number;
            double h = GetArgument(4)._Run(Stack).Number;
            IValue color = GetArgument(5)._Run(Stack).Var;
            bool fill = GetArgument(6)._Run(Stack).Boolean;
            List<(int, int)> poses = new List<(int, int)>();
            GetEquation(x - w/2, x + w/2, y - h/2, y + h/2, poses, (int cx) => {
                double sqrter = 1 - Math.Pow(cx - x, 2.0)/Math.Pow((w/2), 2.0);
                return sqrter < 0 ? y : ((h/2) * Math.Sqrt(sqrter)) + y;
            }, (int cy) => {
                double sqrter = 1 - Math.Pow(cy - y, 2.0)/Math.Pow((h/2), 2.0);
                return sqrter < 0 ? x : ((w/2) * Math.Sqrt(sqrter)) + x;
            });
            GetEquation(x - w/2, x + w/2, y - h/2, y + h/2, poses, (int cx) => {
                double sqrter = 1 - Math.Pow(cx - x, 2.0)/Math.Pow((w/2), 2.0);
                return sqrter < 0 ? y : ((h/2) * -Math.Sqrt(sqrter)) + y;
            }, (int cy) => {
                double sqrter = 1 - Math.Pow(cy - y, 2.0)/Math.Pow((h/2), 2.0);
                return sqrter < 0 ? x : ((w/2) * -Math.Sqrt(sqrter)) + x;
            });
            if(fill) {
                foreach((int, int) pose in poses) {
                    int left = pose.Item1 <= x ? pose.Item1 : (int)Math.Round(x);
                    int wi = (int)Math.Abs(Math.Round(pose.Item1 - x)) + 1;
                    EditPixel(map, startIndex + (height - pose.Item2 - 1) * rowLength, left, pose.Item2, width, bpp, color, wi);
                }
            } else {
                foreach((int, int) pos in poses) {
                    EditPixel(map, startIndex + (height - pos.Item2 - 1) * rowLength, pos.Item1, pos.Item2, width, bpp, color);
                }
            }
        }
    }
}