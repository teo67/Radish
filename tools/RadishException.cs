namespace Tools {
    class RadishExceptionEntry {
        public int Row { get; }
        public int Col { get; }
        public string RMessage { get; set; }
        public RadishExceptionEntry(string message, int row, int col) {
            this.RMessage = message;
            this.Row = row;
            this.Col = col;
        }
    }
    class RadishException : Exception {
        public List<RadishExceptionEntry> Entries { get; }
        public RadishException(string message, int row, int col) {
            this.Entries = new List<RadishExceptionEntry>() {
                new RadishExceptionEntry(message, row, col)
            };
        }
        public RadishException Append(string message, int row, int col) {
            this.Entries.Add(new RadishExceptionEntry(message, row, col));
            return this;
        }
        public void Print() {
            ConsoleColor current = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            string str = "";
            for(int i = 0; i < Entries.Count; i++) {
                if(i == 1) {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                Console.WriteLine($"{str}{Entries[i].RMessage}{((Entries[i].Row == -1 || Entries[i].Col == -1) ? "" : $" [row {Entries[i].Row}, column {Entries[i].Col}]")}");
                str += "  ";
            }
            Console.ForegroundColor = current;
        }
        public RadishException AppendToTop(string message) {
            this.Entries[this.Entries.Count - 1].RMessage += message;
            return this;
        }
    }
}