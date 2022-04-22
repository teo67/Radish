namespace Tools {
    class CountingReader {
        private StreamReader Container { get; }
        public int row;
        public int col;
        public static string Path { get; set; }
        static CountingReader() {
            Path = "";
        }
        public CountingReader(string filename) {
            Container = new StreamReader(Path + filename);
            row = 1;
            col = 1;
        }
        public bool EndOfStream {
            get {
                return Container.EndOfStream;
            }
        }
        public char Peek() {
            return (char)Container.Peek();
        }
        public void Read() {
            char returning = (char)Container.Read();
            if(returning == '\r') {
                Container.Read(); // carriage return on windows
                row++;
                col = 1;
            } else if(returning == '\n') { // line return on mac
                row++;
                col = 1;
            } else {
                col++;
            }
        }
        public RadishException Error(string msg, int rowNum, int colNum) {
            return new RadishException(msg, rowNum, colNum);
        }
        public RadishException Error(string msg) {
            return Error(msg, row, col);
        }
    }
}