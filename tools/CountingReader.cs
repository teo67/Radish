namespace Tools {
    class CountingReader : IReader {
        private StreamReader Container { get; }
        public int row { get; private set; }
        public int col { get; private set; }
        public string Filename { get; }
        public CountingReader(string filename) {
            Container = new StreamReader(filename);
            row = 1;
            col = 1;
            Filename = filename;
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