namespace Tools.Operators {
    class GenerateBMP : Operator {
        private IOperator Width { get; }
        private IOperator Height { get; }
        private IOperator BPP { get; }
        private IOperator Pallette { get; }
        public GenerateBMP(IOperator width, IOperator height, IOperator bpp, IOperator pallette) : base(-1, -1) {
            Width = width;
            Height = height;
            BPP = bpp;
            Pallette = pallette;
        }
        public override IValue Run(Stack Stack) {
            int width = (int)Width._Run(Stack).Number;
            int height = (int)Height._Run(Stack).Number;
            int bpp = (int)BPP._Run(Stack).Number;
            bool pallette = Pallette._Run(Stack).Boolean;
            int rowLength = (int)Math.Ceiling(Math.Ceiling((bpp * width) / 8.0) / 4.0) * 4;
            int offset = 14 + 40;
            if(pallette) {
                offset += 4 * (1 << bpp);
            }
            int totalSize = rowLength * height + offset;
            byte[] writing = new byte[totalSize];
            // file header (14), info header (40), pallette (8), data (variable)
            // file header
            // file type (BM)
            writing[0] = 0x42;
            writing[1] = 0x4D;
            // file size
            byte[] fileSize = BitConverter.GetBytes(totalSize);
            if(!BitConverter.IsLittleEndian) {
                Array.Reverse(fileSize);
            }
            if(fileSize.Length > 4) {
                throw new RadishException("The BMP file size is too large!", Row, Col);
            }
            for(int i = 0; i < 4; i++) {
                writing[i + 2] = fileSize[i];
            }
            // 4 reserved bytes
            // reserve 6, 7, 8, 9
            // pixel data offset
            byte[] tOffset = BitConverter.GetBytes(offset);
            if(!BitConverter.IsLittleEndian) {
                Array.Reverse(tOffset);
            }
            if(tOffset.Length > 4) {
                throw new RadishException("The offset size is too large!", Row, Col);
            }
            for(int i = 0; i < 4; i++) {
                writing[i + 10] = tOffset[i];
            }
            // END OF FILE HEADER
            // info header
            // header size
            writing[14] = 40;
            // 15, 16, and 17 stay at 0
            // width and height of image
            byte[] tWidth = BitConverter.GetBytes(width);
            byte[] tHeight = BitConverter.GetBytes(height);
            if(!BitConverter.IsLittleEndian) {
                Array.Reverse(tWidth);
                Array.Reverse(tHeight);
            }
            if(tWidth.Length > 4 || tHeight.Length > 4) {
                throw new RadishException("The image size is too large!", Row, Col);
            }
            for(int i = 0; i < 4; i++) {
                writing[i + 18] = tWidth[i];
                writing[i + 22] = tHeight[i];
            } // this reserves 18, 19, 20, 21, 22, 23, 24, 25
            // color planes
            writing[26] = 1;
            // 27 stays at 0
            // bpp
            writing[28] = 1;
            // 29 stays at 0
            // 30, 31, 32, 33 stay at 0 for compression
            // 34, 35, 36, 37 stay at 0 for default image size
            // 38, 39, 40, 41, 42, 43, 44, 45 stay at 0 for xpermeter and ypermeter
            // 46, 47, 48, 49 stay at 0 for default total colors (2^1) => 2
            // 50, 51, 52, 53 stay at 0 for no important color preference
            // END OF INFO HEADER
            // pallette
            // reverse 54 through {offset - 1}
            // everything else stays default
            foreach(byte by in writing) {
                Console.Write(by + " ");
            }
            Console.Write("\n" + writing.Length);
            string converted = System.Text.Encoding.Unicode.GetString(writing, 0, writing.Length);
            return new Values.StringLiteral(converted);
        }
        public override string Print() {
            return $"createBMP(width = {Width.Print()}, height = {Height.Print()})";
        }
    }
}