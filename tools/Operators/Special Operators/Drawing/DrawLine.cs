namespace Tools.Operators {
    class DrawLine : BMPEditor {
        public DrawLine(Librarian librarian) : base("drawLine", 5, librarian) {}
        protected override void RunBMP(byte[] map, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {
            double x1 = GetArgument(1)._Run(Stack).Number;
            double y1 = GetArgument(2)._Run(Stack).Number;
            double x2 = GetArgument(3)._Run(Stack).Number;
            double y2 = GetArgument(4)._Run(Stack).Number;
            IValue color = GetArgument(5)._Run(Stack).Var;
            double xmin = Math.Min(x1, x2); // sort x1, x2, y1, y2
            double ymin = Math.Min(y1, y2);
            
            List<(int, int)> poses = new List<(int, int)>();
                double m1 = 0;
                double m2 = 0;
                if(x2 != x1) {
                    m1 = (y2 - y1) / (x2 - x1);
                }
                if(y2 != y1) {
                    m2 = (x2 - x1) / (y2 - y1);
                }
                GetEquation(xmin, xmin == x1 ? x2 : x1, ymin, ymin == y1 ? y2 : y1, poses, 
                ((x1 != x2) ? ((int x) => {
                    return Math.Round(m1 * (x - x1)) + y1;
                }) : null),
                (y1 != y2) ? ((int y) => {
                    return Math.Round(m2 * (y - y1)) + x1;
                }) : null);

            foreach((int, int) pos in poses) {
                EditPixel(map, startIndex + (height - pos.Item2 - 1) * rowLength, pos.Item1, pos.Item2, width, bpp, color);
            }
        }
        
    }
}