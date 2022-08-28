namespace Tools.Operators {
    class DrawPoint : BMPEditor {
        public DrawPoint(Librarian librarian) : base("drawPoint", 3, librarian) {}
        protected override void RunBMP(byte[] map, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {
            int x = (int)GetArgument(1)._Run(Stack).Number;
            int y = (int)GetArgument(2)._Run(Stack).Number;
            int color = (int)GetArgument(3)._Run(Stack).Number;
            EditPixel(map, startIndex + (height - y - 1) * rowLength, x, width, bpp, new List<int>() { color });
        }
        
    }
}