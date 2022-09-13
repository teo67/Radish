namespace Tools.Operators {
    class DrawRectangle : BMPEditor {
        public DrawRectangle(Librarian librarian) : base("drawRectangle", 6, librarian) {}
        protected override void RunBMP(byte[] map, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {
            double rawX = GetArgument(1)._Run(Stack).Number;
            double w = GetArgument(3)._Run(Stack).Number;
            int finalX = (int)Math.Round(rawX + w, 0, MidpointRounding.AwayFromZero);
            double rawY = GetArgument(2)._Run(Stack).Number;
            double h = GetArgument(4)._Run(Stack).Number;
            int finalY = (int)Math.Round(rawY + h, 0, MidpointRounding.AwayFromZero);
            // finals are 1 past where they should be

            int rx = (int)Math.Round(rawX, 0, MidpointRounding.AwayFromZero);
            int ry = (int)Math.Round(rawY, 0, MidpointRounding.AwayFromZero);
            // int x = (int)Math.Round(GetArgument(1)._Run(Stack).Number, 0, MidpointRounding.AwayFromZero);
            // int y = (int)Math.Round(GetArgument(2)._Run(Stack).Number, 0, MidpointRounding.AwayFromZero);
            // int w = (int)Math.Round(GetArgument(3)._Run(Stack).Number, 0, MidpointRounding.AwayFromZero);
            // double oh = GetArgument(4)._Run(Stack).Number;
            // int h = (int)Math.Round(oh, 0, MidpointRounding.AwayFromZero);
            IValue color = GetArgument(5)._Run(Stack).Var;
            bool fill = GetArgument(6)._Run(Stack).Boolean;
            EditPixel(map, startIndex + ((height - ry - 1) * rowLength), rx, ry, width, bpp, color, finalX - rx);
            for(int i = ry + 1; i < finalY - 1; i++) {
                int ypos = startIndex + ((height - i - 1) * rowLength);
                EditPixel(map, ypos, rx, i, width, bpp, color);
                if(fill && w > 2) {
                    EditPixel(map, ypos, rx + 1, i, width, bpp, color, finalX - rx - 2 );
                }
                EditPixel(map, ypos, finalX - 1, i, width, bpp, color );
            }
            EditPixel(map, startIndex + ((height - finalY) * rowLength), rx, finalY - 1, width, bpp, color, finalX - rx);
        }
        public override string Print() {
            return $"drawRect({GetArgument(1)}, {GetArgument(2)}, {GetArgument(3)}, {GetArgument(4)}, {GetArgument(5)})";
        }
    }
}