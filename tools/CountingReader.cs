namespace Tools {
    class CountingReader {
        private StreamReader Container { get; }
        private int lineNumber;
        private int colNumber;
        public CountingReader(string filename) {
            Container = new StreamReader(filename);
            lineNumber = 1;
            colNumber = -1;
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
            if(returning == '\n' || returning == '\r') {
                lineNumber++;
                colNumber = -1;
            } else {
                colNumber++;
            }
        }
        public Exception Error(string msg) {
            return new Exception($"[line {lineNumber}, column {colNumber}]: {msg}");
        }
    }
}