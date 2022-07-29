namespace Tools {
    class StringReader : IReader {
        private string Input { get; }
        private int Index { get; set; }
        public StringReader(string input) {
            this.Input = input;
            this.Index = 0;
        }
        public int row {
            get {
                return 0;
            }
        }
        public int col {
            get {
                return 0;
            }
        }
        public string Filename {
            get {
                return "anonymous file";
            }
        }
        public bool EndOfStream {
            get {
                return Index >= Input.Length;
            }
        }
        public char Peek() {
            return Input[Index];
        }
        public void Read() {
            Index++;
        }
        public RadishException Error(string msg, int row, int col) {
            return new RadishException(msg, row, col);
        }
        public RadishException Error(string msg) {
            return Error(msg, 0, 0);
        }
    }
}