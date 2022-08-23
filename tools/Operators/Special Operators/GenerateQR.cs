using System.Text;

namespace Tools.Operators {
    class GenerateQR : Operator {
        private static List<List<byte>> GeneratorPolynomials = new List<List<byte>>() { new List<byte>() { 0, 0 } };
        private static List<byte> powers = new List<byte>() { 1 };
        private static string[] formatStrings = new string[8];
        private static string[] versionStrings = new string[34];
        private static Func<int, int, bool>[] masks;
        static GenerateQR() {
            masks = new Func<int, int, bool>[] {
                (int x, int y) => {
                    return (y + x) % 2 == 0;
                }, (int x, int y) => {
                    return y % 2 == 0;
                }, (int x, int y) => {
                    return x % 3 == 0;
                }, (int x, int y) => {
                    return (y + x) % 3 == 0;
                }, (int x, int y) => {
                    return (Math.Floor((double)(y / 2)) + Math.Floor((double)(x / 3))) % 2 == 0;
                }, (int x, int y) => {
                    return ((x * y) % 2) + ((x * y) % 3) == 0;
                }, (int x, int y) => {
                    return (((x * y) % 2) + ((x * y) % 3)) % 2 == 0;
                }, (int x, int y) => {
                    return (((x + y) % 2) + ((x * y) % 3)) % 2 == 0;
                }
            };
            byte starting = 1;
            for(int i = 1; i < 255; i++) {
                byte saved = starting;
                starting *= 2;
                if(starting < saved) {
                    starting = (byte)(starting ^ 29);
                }
                powers.Add(starting);
            }
            for(UInt16 maskNumber = 0; maskNumber < 8; maskNumber++) {
                UInt16 formatString = 24; // 11000
                formatString += maskNumber; // 11XXX
                formatString *= 1024; // 11XXX0000000000
                UInt16 originalFormat = formatString;
                while(formatString >= 1024) {
                    UInt16 generator = 1335; // 10100110111
                    while(Math.Floor(Math.Log2(formatString)) > Math.Floor(Math.Log2(generator))) {
                        generator *= 2;
                    }
                    formatString ^= generator;
                }
                formatString = (UInt16)(originalFormat + formatString);
                formatString ^= 21522;
                string converted = Convert.ToString(formatString, 2).PadLeft(15, '0');
                formatStrings[maskNumber] = converted;
            }
            for(UInt32 versionNumber = 7; versionNumber <= 40; versionNumber++) {
                UInt32 versionString = versionNumber;
                versionString *= 4096; 
                UInt32 originalVersion = versionString;
                while(versionString >= 4096) {
                    UInt32 generator = 7973; // 1111100100101
                    while(Math.Floor(Math.Log2(versionString)) > Math.Floor(Math.Log2(generator))) {
                        generator *= 2;
                    }
                    versionString ^= generator;
                }
                versionString = (UInt32)(originalVersion + versionString);
                string converted = Convert.ToString(versionString, 2).PadLeft(18, '0');
                versionStrings[versionNumber - 7] = converted;
            }
        }
        private IOperator Input { get; }
        private IOperator OutFile { get; }
        private IOperator ModuleSize { get; }
        public GenerateQR(IOperator input, IOperator outFile, IOperator moduleSize) : base(-1, -1) {
            Input = input;
            OutFile = outFile;
            ModuleSize = moduleSize;
        }
        private byte GetExponent(byte input) {
            int index = powers.IndexOf(input);
            if(index == -1) {
                throw new Exception("That don't exist chief!!!");
            }
            return (byte)index;
        }
        private byte Add(byte a, byte b) {
            return (byte)((a + b) % 255);
        }
        private void GeneratePolynomial() {
            List<byte> last = GeneratorPolynomials[GeneratorPolynomials.Count - 1];
            byte i = (byte)GeneratorPolynomials.Count;
            List<byte> next = new List<byte>();
            for(int j = 0; j < last.Count; j++) {
                if(j == 0) {
                    next.Add(Add(last[0], i));
                } else {
                    next.Add(GetExponent((byte)(powers[Add(last[j], i)] ^ powers[(last[j - 1])])));
                }
            }
            next.Add(0);
            GeneratorPolynomials.Add(next);
        }
        private void GetQR(byte[] full, string fileName, int moduleSize, int start = 0, int numQR = 0) {
            int index = -1;
            for(int i = 1; i < 41; i++) {
                if(VersionInfo.GetVersion(i).Capacity >= full.Length - start) {
                    index = i;
                    break;
                }
            }
            if(index == -1) {
                index = 40;
                GetQR(full, fileName, start + VersionInfo.GetVersion(40).Capacity, numQR + 1);
            }
            VersionInfo info = VersionInfo.GetVersion(index);
            string content = "0100";
            int charcount = Encoding.UTF8.GetDecoder().GetCharCount(full, start, Math.Min(full.Length - start, info.Capacity));
            content += Convert.ToString(charcount, 2).PadLeft(index < 10 ? 8 : 16, '0'); // character indicator bit value
            for(int i = start; i < start + info.Capacity; i++) {
                if(i >= full.Length) {
                    continue;
                }   
                content += Convert.ToString(full[i], 2).PadLeft(8, '0');
            }
            int withpad = 8 * (info.Capacity + (index < 10 ? 2 : 3));
            if(content.Length < withpad) {
                content += new string('0', Math.Min(4, withpad - content.Length));
            }
            if(content.Length % 8 != 0) {
                content += new string('0', 8 - (content.Length % 8));
            }
            while(content.Length < withpad) {
                content += "11101100";
                if(content.Length >= withpad) {
                    break;
                }
                content += "00010001";
            }
            string withoutEnd = fileName.EndsWith(".bmp") ? fileName.Substring(0, fileName.Length - 4) : fileName;
            HandleInput(content, index, info, numQR == 0 ? $"{withoutEnd}.bmp" : $"{withoutEnd} ({numQR}).bmp", moduleSize);
        }
        private List<int> DeterminePatterns(int version) {
            if(version == 1) {
                return new List<int>();
            }
            int diff = (version + 1) * 4;
            int numDivisions = 1;
            while(diff > 28) {
                diff = (diff * numDivisions) / (numDivisions + 1);
                numDivisions++;
            }
            List<int> returning = new List<int>();
            for(int i = 0; i <= numDivisions; i++) {
                returning.Add(i * diff + 6);
            }
            return returning;
        }
        private void HandleInput(string input, int version, VersionInfo info, string fileName, int moduleSize) {
            List<int> alignPatterns = DeterminePatterns(version);
            int size = (version - 1) * 4 + 21;

            string final = "";
            int group2per = info.Group1Per + 1;
            for(int i = 0; i < group2per; i++) {
                if(i < info.Group1Per) {
                    for(int j = 0; j < info.Group1Blocks; j++) {
                        final += input.Substring(j * info.Group1Per * 8 + i * 8, 8);
                    }
                }
                for(int j = 0; j < info.Group2Blocks; j++) {
                    final += input.Substring(info.Group1Per * info.Group1Blocks * 8 + j * group2per * 8 + i * 8, 8);
                }
            }
            string[] errorcodes = new string[info.Group1Blocks + info.Group2Blocks];
            for(int i = 0; i < info.Group1Blocks; i++) {
                errorcodes[i] = getCodewords(i * info.Group1Per * 8, info.Group1Per, info.ErrorDigits, input);
            }
            for(int i = 0; i < info.Group2Blocks; i++) {
                errorcodes[info.Group1Blocks + i] = getCodewords(info.Group1Blocks * info.Group1Per * 8 + i * group2per * 8, group2per, info.ErrorDigits, input);
            }
            for(int i = 0; i < info.ErrorDigits; i++) {
                for(int j = 0; j < errorcodes.Length; j++) {
                    final += errorcodes[j].Substring(i * 8, 8);
                }
            }
            final += new string('0', info.Remainder);
            bool[,] bitmap = new bool[size, size];
            List<(int, int)> invalid = new List<(int, int)>();
            PlaceFinder(0, 0, bitmap, invalid);
            PlaceFinder(size - 7, 0, bitmap, invalid);
            PlaceFinder(0, size - 7, bitmap, invalid);
            PlaceRect(0, 7, 8, 1, bitmap, invalid);
            PlaceRect(7, 0, 1, 7, bitmap, invalid);
            PlaceRect(size - 8, 7, 8, 1, bitmap, invalid);
            PlaceRect(size - 8, 0, 1, 7, bitmap, invalid);
            PlaceRect(0, size - 8, 8, 1, bitmap, invalid);
            PlaceRect(7, size - 7, 1, 7, bitmap, invalid);
            foreach(int x in alignPatterns) {
                foreach(int y in alignPatterns) {
                    PlaceAlignPattern(x, y, bitmap, invalid);
                }
            }
            for(int i = 9; i < size - 8; i++) {
                if(!invalid.Contains((6, i))) {
                    PlaceInvalid(6, i, i % 2 == 1, bitmap, invalid);
                }
                if(!invalid.Contains((i, 6))) {
                    PlaceInvalid(i, 6, i % 2 == 1, bitmap, invalid);
                }
            }
            PlaceRect(0, 8, 9, 1, bitmap, invalid, false); // format info zones
            PlaceRect(8, 0, 1, 8, bitmap, invalid, false);
            PlaceRect(size - 8, 8, 8, 1, bitmap, invalid, false);
            PlaceRect(8, size - 8, 1, 8, bitmap, invalid, false);
            if(version >= 7) { // version info zones
                string versionString = versionStrings[version - 7];
                for(int i = 0; i < 6; i++) {
                    for(int j = 0; j < 3; j++) {
                        PlaceInvalid(size - 11 + j, i, versionString[17 - (i * 3 + j)] == '0', bitmap, invalid);
                        PlaceInvalid(i, size - 11 + j, versionString[17 - (i * 3 + j)] == '0', bitmap, invalid);
                    }
                }
            }
            // bit placement starts here
            int currentX = size - 1;
            int currentY = size - 1;
            int direction = -1;
            int currentBit = 0;
            while(currentX >= 0) {
                currentBit += TryPlace(currentX, currentY, bitmap, invalid, final, currentBit);
                currentBit += TryPlace(currentX - 1, currentY, bitmap, invalid, final, currentBit);
                currentY += direction;
                if(currentY < 0 || currentY >= size) {
                    currentY -= direction;
                    currentX -= currentX == 8 ? 3 : 2; // exception for vertical timing pattern
                    direction *= -1;
                }
            }
            bool[,] best = bitmap;
            int? bestScore = null;
            for(UInt16 i = 0; i < masks.Length; i++) {
                bool[,] creating = new bool[size, size];
                for(int y = 0; y < size; y++) {
                    for(int x = 0; x < size; x++) {
                        creating[x, y] = (masks[i](x, y) && !invalid.Contains((x, y))) ? (!bitmap[x, y]) : bitmap[x, y];
                    }
                }
                string maskString = formatStrings[i];
                for(int j = 0; j < 7; j++) {
                    creating[j == 6 ? 7 : j, 8] = maskString[j] == '0';
                    creating[8, size - j - 1] = maskString[j] == '0';
                }
                for(int j = 7; j < 15; j++) {
                    creating[8, 14 - (j < 9 ? j - 1 : j)] = maskString[j] == '0';
                    creating[size - 15 + j, 8] = maskString[j] == '0';
                }
                int res = GetPenalty(creating, size);
                if(bestScore == null || res < bestScore) {
                    bestScore = res;
                    best = creating;
                }
            }
            //RenderMap(best, size);
            createBMP(best, size, fileName, moduleSize);
        }
        private void createBMP(bool[,] bitmap, int size, string fileName, int moduleSize) {
            int rowLength = (int)(Math.Ceiling(Math.Ceiling((moduleSize + (double)((size * moduleSize) / 8.0))) / 4.0) * 4);
            int totalSize = 14 + 40 + 8 + (rowLength * (size + 8) * moduleSize);
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
                throw new Exception("The BMP file size is too large!");
            }
            for(int i = 0; i < 4; i++) {
                writing[i + 2] = fileSize[i];
            }
            // 4 reserved bytes
            // reserve 6, 7, 8, 9
            // pixel data offset
            writing[10] = 62;
            // using little endian, so 11, 12, and 13 stay at 0
            // END OF FILE HEADER
            // info header
            // header size
            writing[14] = 40;
            // 15, 16, and 17 stay at 0
            // width and height of image
            byte[] translatedSize = BitConverter.GetBytes(moduleSize * (size + 8));
            if(!BitConverter.IsLittleEndian) {
                Array.Reverse(translatedSize);
            }
            if(translatedSize.Length > 4) {
                throw new Exception("The image size is too large!");
            }
            for(int i = 0; i < 4; i++) {
                writing[i + 18] = translatedSize[i];
                writing[i + 22] = translatedSize[i];
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
            // 54, 55, 56, 57 stay at 0 to symbolize black
            writing[58] = 255;
            writing[59] = 255;
            writing[60] = 255;
            // 61 stays at 0 for the end of the white byte
            // END PALLETTE
            // pixel data
            
            for(int y = 0; y < size; y++) {
                byte currentByte = 0;
                int count = 7;
                int numBytes = 0;
                for(int x = -4; x < size + 4; x++) {
                    for(int k = 0; k < moduleSize; k++) {
                        if(x < 0 || (x >= size && x < size + 4) || (x < size && bitmap[x, size - y - 1])) {
                            currentByte |= (byte)(1 << count);
                        }
                        count--;
                        if(count == -1) {
                            count = 7;
                            for(int writeY = 0; writeY < moduleSize; writeY++) {
                                writing[((y + 4) * moduleSize + writeY) * rowLength + numBytes + 62] = currentByte;
                            }
                            currentByte = 0;
                            numBytes++;
                        }
                    }
                }
                if(currentByte > 0 && numBytes < rowLength) {
                    for(int writeY = 0; writeY < moduleSize; writeY++) {
                        writing[((y + 4) * moduleSize + writeY) * rowLength + numBytes + 62] = currentByte;
                    }
                }
            }
            for(int i = 0; i < rowLength; i++) {
                for(int y = 0; y < 4 * moduleSize; y++) {
                    writing[y * rowLength + 62 + i] = 255;
                    writing[(((size + 8) * moduleSize) - y - 1) * rowLength + 62 + i] = 255;
                }
            }
            // END PIXEL DATA
            File.WriteAllBytes(fileName, writing);
        }

        private string getCodewords(int startPosition, int numBytes, int numdigits, string input) {
            List<byte> converted = new List<byte>();
            for(int i = 0; i < numdigits; i++) {
                converted.Add(0);
            }
            for(int i = startPosition + numBytes * 8; i > startPosition; i -= 8) {
                converted.Add(Convert.ToByte(input.Substring(i - 8, 8), 2));
            }
            while(GeneratorPolynomials.Count < numdigits) {
                GeneratePolynomial();
            }
            List<byte> selected = GeneratorPolynomials[numdigits - 1];
            while(converted.Count > numdigits) {
                byte toAlpha = GetExponent(converted[converted.Count - 1]);
                converted.RemoveAt(converted.Count - 1);
                for(int j = 0; j < selected.Count - 1; j++) {
                    byte added = Add(toAlpha, selected[j]);
                    byte unAlpha = powers[added];
                    converted[converted.Count - selected.Count + j + 1] = (byte)(converted[converted.Count - selected.Count + j + 1] ^ unAlpha);
                }
                while(converted.Count > numdigits && converted[converted.Count - 1] == 0) {
                    converted.RemoveAt(converted.Count - 1);
                }
            }
            string errorCodewords = "";
            for(int i = numdigits - 1; i >= 0; i--) {
                errorCodewords += Convert.ToString(converted[i], 2).PadLeft(8, '0');
            }
            return errorCodewords;
        }
        private void PlaceInvalid(int x, int y, bool input, bool[,] bitmap, List<(int, int)> invalid) {
            bitmap[x, y] = input;
            invalid.Add((x, y));
        }
        private void PlaceFinder(int x, int y, bool[,] bitmap, List<(int, int)> invalid) {
            for(int i = 0; i < 7; i++) {
                PlaceInvalid(x, y + i, false, bitmap, invalid);
                PlaceInvalid(x + 1, y + i, i > 0 && i < 6, bitmap, invalid);
                for(int j = 0; j < 3; j++) {
                    PlaceInvalid(x + 2 + j, y + i, i == 1 || i == 5, bitmap, invalid);
                }
                PlaceInvalid(x + 5, y + i, i > 0 && i < 6, bitmap, invalid);
                PlaceInvalid(x + 6, y + i, false, bitmap, invalid);
            }
        }
        private void PlaceRect(int x, int y, int w, int h, bool[,] bitmap, List<(int, int)> invalid, bool input = true) {
            for(int i = 0; i < w; i++) {
                for(int j = 0; j < h; j++) {
                    PlaceInvalid(x + i, y + j, input, bitmap, invalid);
                }
            }
        }
        private void PlaceAlignPattern(int x, int y, bool[,] bitmap, List<(int, int)> invalid) {
            for(int i = -2; i < 2; i++) {
                for(int j = -2; j < 2; j++) {
                    if(invalid.Contains((x + i, y + j))) {
                        return;
                    }
                }
            }
            for(int i = -2; i < 3; i++) {
                PlaceInvalid(x - 2, y + i, false, bitmap, invalid);
                PlaceInvalid(x - 1, y + i, i > -2 && i < 2, bitmap, invalid);
                PlaceInvalid(x, y + i, i == -1 || i == 1, bitmap, invalid);
                PlaceInvalid(x + 1, y + i, i > -2 && i < 2, bitmap, invalid);
                PlaceInvalid(x + 2, y + i, false, bitmap, invalid);
            }
        }

        private int TryPlace(int x, int y, bool[,] bitmap, List<(int, int)> invalid, string final, int currentBit) {
            if(!invalid.Contains((x, y))) {
                bitmap[x, y] = final[currentBit] == '0';
                return 1;
            }
            return 0;
        }

        private int GetPenalty(bool[,] map, int size) {
            int score = 0;
            int numDark = 0;
            for(int y = 0; y < size; y++) {
                int inARow = 0;
                bool current = false;
                for(int x = 0; x < size; x++) {
                    if(!map[x, y]) {
                        numDark++;
                    }
                    if(map[x, y] == current) {
                        inARow++;
                    } else {
                        score += inARow < 5 ? 0 : inARow - 2; // add for every consecutive pattern in row
                        inARow = 1;
                        current = !current;
                    }
                    if(x < size - 1 && y < size - 1) {
                        if(map[x, y] == map[x + 1, y] && map[x, y] == map[x, y + 1] && map[x, y] == map[x + 1, y + 1]) {
                            score += 3; // add 3 for every 2x2 square of same
                        }
                    }
                    if(x < size - 6) {
                        if(!map[x, y] && map[x + 1, y] && !map[x + 2, y] && !map[x + 3, y] && !map[x + 4, y] && map[x + 5, y] && !map[x + 6, y]) { // add 40 if OOOOXOXXXOX or XOXXXOXOOOO is found horizontally
                            bool cancel = false;
                            if(x >= 4) {
                                if(map[x - 4, y] && map[x - 3, y] && map[x - 2, y] && map[x - 1, y]) {
                                    score += 40;
                                    cancel = true;
                                }
                            }
                            if(x < size - 10 && !cancel) {
                                if(map[x + 7, y] && map[x + 8, y] && map[x + 9, y] && map[x + 10, y]) {
                                    score += 40;
                                }
                            }
                        }
                    }
                    if(y < size - 6) {
                        if(!map[x, y] && map[x, y + 1] && !map[x, y + 2] && !map[x, y + 3] && !map[x, y + 4] && map[x, y + 5] && !map[x, y + 6]) { // add 40 if OOOOXOXXXOX or XOXXXOXOOOO is found vertically 
                            bool cancel = false;
                            if(y >= 4) {
                                if(map[x, y - 4] && map[x, y - 3] && map[x, y - 2] && map[x, y - 1]) {
                                    score += 40;
                                    cancel = true;
                                }
                            }
                            if(y < size - 10 && !cancel) {
                                if(map[x, y + 7] && map[x, y + 8] && map[x, y + 9] && map[x, y + 10]) {
                                    score += 40;
                                }
                            }
                        }
                    }
                }
                score += inARow < 5 ? 0 : inARow - 2;
            }
            for(int x = 0; x < size - 1; x++) {
                int inACol = 0;
                bool current = false;
                for(int y = 0; y < size - 1; y++) {
                    if(map[x, y] == current) {
                        inACol++;
                    } else {
                        score += inACol < 5 ? 0 : inACol - 2; // add for every consecutive pattern in column
                        inACol = 1;
                        current = !current;
                    }
                }
                score += inACol < 5 ? 0 : inACol - 2;
            }
            double outOf20 = 20 * numDark / (size * size);
            int closestToMid = outOf20 < 10 ? (int)Math.Ceiling(outOf20) : (int)Math.Floor(outOf20);
            score += Math.Abs(10 - closestToMid) * 10; // add based on ratio of black to white
            return score;
        }

        private void RenderMap(bool[,] map, int size) {
            for(int i = 0; i < size; i++) {
                for(int j = 0; j < size; j++) {
                    Console.Write(map[j, i] ? "O " : "_ ");
                }
                Console.Write("\n");
            }
        }

        public override IValue Run(Stack Stack) {
            GetQR(Encoding.Latin1.GetBytes(Input._Run(Stack).String), OutFile._Run(Stack).String, (int)Math.Round(ModuleSize._Run(Stack).Number));
            return new Values.NoneLiteral();
        }

        public override string Print() {
            return $"generateQR({Input.Print()}, {OutFile.Print()})";
        }
    }
}
