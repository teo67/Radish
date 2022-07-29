namespace Tools {
    interface IReader {
        public int row { get; }
        public int col { get; }
        public string Filename { get; }
        public bool EndOfStream { get; }
        public char Peek();
        public void Read();
        public RadishException Error(string msg, int rowNum, int colNum);
        public RadishException Error(string msg);
    }
}