namespace Tools.Operators {
    class DrawRectangle : BMPEditor {
        public DrawRectangle(Librarian librarian) : base("drawRectangle", 6, librarian) {}
        protected override void RunBMP(byte[] map, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {
            int x = (int)GetArgument(1)._Run(Stack).Number;
            int y = (int)GetArgument(2)._Run(Stack).Number;
            int w = (int)GetArgument(3)._Run(Stack).Number;
            int h = (int)GetArgument(4)._Run(Stack).Number;
            int color = (int)GetArgument(5)._Run(Stack).Number;
            bool fill = GetArgument(6)._Run(Stack).Boolean;
            EditPixel(map, startIndex + ((height - y - 1) * rowLength), x, width, bpp, new List<int>() { color }, new List<int>() { w });
            for(int i = 0; i < h; i++) {
                int ypos = startIndex + ((height - y - i - 1) * rowLength);
                EditPixel(map, ypos, x, width, bpp, new List<int>() { color });
                if(fill && w > 2) {
                    EditPixel(map, ypos, x + 1, width, bpp, new List<int>() { color }, new List<int>() { w - 2 });
                }
                EditPixel(map, ypos, x + w - 1, width, bpp, new List<int>() { color });
            }
            EditPixel(map, startIndex + ((height - y - h) * rowLength), x, width, bpp, new List<int>() { color }, new List<int>() { w });
        }
        public override string Print() {
            return $"drawRect({GetArgument(1)}, {GetArgument(2)}, {GetArgument(3)}, {GetArgument(4)}, {GetArgument(5)})";
        }
    }
}