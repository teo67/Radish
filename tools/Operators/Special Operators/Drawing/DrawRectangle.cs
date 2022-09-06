namespace Tools.Operators {
    class DrawRectangle : BMPEditor {
        public DrawRectangle(Librarian librarian) : base("drawRectangle", 6, librarian) {}
        protected override void RunBMP(byte[] map, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {
            int x = (int)Math.Round(GetArgument(1)._Run(Stack).Number, 0, MidpointRounding.AwayFromZero);
            int y = (int)Math.Round(GetArgument(2)._Run(Stack).Number, 0, MidpointRounding.AwayFromZero);
            int w = (int)Math.Round(GetArgument(3)._Run(Stack).Number, 0, MidpointRounding.AwayFromZero);
            double oh = GetArgument(4)._Run(Stack).Number;
            int h = (int)Math.Round(oh, 0, MidpointRounding.AwayFromZero);
            IValue color = GetArgument(5)._Run(Stack).Var;
            bool fill = GetArgument(6)._Run(Stack).Boolean;
            EditPixel(map, startIndex + ((height - y - 1) * rowLength), x, y, width, bpp, color, w );
            for(int i = 0; i < h; i++) {
                int ypos = startIndex + ((height - y - i - 1) * rowLength);
                EditPixel(map, ypos, x, y + i, width, bpp, color);
                if(fill && w > 2) {
                    EditPixel(map, ypos, x + 1, y + i, width, bpp, color, w - 2 );
                }
                EditPixel(map, ypos, x + w - 1, y + i, width, bpp, color );
            }
            EditPixel(map, startIndex + ((height - y - h) * rowLength), x, y + h - 1, width, bpp, color, w);
        }
        public override string Print() {
            return $"drawRect({GetArgument(1)}, {GetArgument(2)}, {GetArgument(3)}, {GetArgument(4)}, {GetArgument(5)})";
        }
    }
}