namespace Tools {
    class CountingReader {
        private StreamReader Container { get; }
        public int row;
        public int col;
        public CountingReader(string filename) {
            Container = new StreamReader(filename);
            row = 1;
            col = 0;
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
                col = 0;
            } else {
                col++;
            }
        }
        public Exception Error(string msg, int rowNum, int colNum) {
            return new Exception($"[line {rowNum}, column {colNum}]: {msg}");
        }
        public Exception Error(string msg) {
            return Error(msg, row, col);
        }
    }
}