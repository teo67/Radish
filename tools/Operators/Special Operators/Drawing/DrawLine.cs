namespace Tools.Operators {
    class DrawLine : BMPEditor {
        public DrawLine(Librarian librarian) : base("drawLine", 5, librarian) {}
        protected override void RunBMP(byte[] map, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {
            int x1 = (int)GetArgument(1)._Run(Stack).Number;
            int y1 = (int)GetArgument(2)._Run(Stack).Number;
            int x2 = (int)GetArgument(3)._Run(Stack).Number;
            int y2 = (int)GetArgument(4)._Run(Stack).Number;
            int color = (int)GetArgument(5)._Run(Stack).Number;
            int min = Math.Min(x1, x2); // sort x1, x2, y1, y2
            if(x2 == min) {
                x2 = x1;
            }
            x1 = min;
            min = Math.Min(y1, y2);
            if(y2 == min) {
                y2 = y1;
            }
            y1 = min;
            
            List<(int, int)> poses = new List<(int, int)>();
                double m1 = 0;
                double m2 = 0;
                if(x2 != x1) {
                    m1 = (double)(y2 - y1) / (x2 - x1);
                }
                if(y2 != y1) {
                    m2 = (double)(x2 - x1) / (y2 - y1);
                }
                GetEquation(x1, x2, y1, y2, poses, 
                ((x1 != x2) ? ((int x) => {
                    return Math.Round(m1 * (x - x1)) + y1;
                }) : null),
                (y1 != y2) ? ((int y) => {
                    return Math.Round(m2 * (y - y1)) + x1;
                }) : null);

            foreach((int, int) pos in poses) {
                EditPixel(map, startIndex + (height - pos.Item2 - 1) * rowLength, pos.Item1, width, bpp, new List<int>() { color });
            }
        }
        
    }
}