namespace Tools {
    class RadishExceptionEntry {
        public int Row { get; }
        public int Col { get; }
        public string FileName { get; }
        public string RMessage { get; set; }
        public bool Flagged { get; }
        public RadishExceptionEntry(string message, int row, int col, string fileName, bool flagged = true) {
            this.RMessage = message;
            this.Row = row;
            this.Col = col;
            this.FileName = fileName;
            this.Flagged = flagged;
        }
    }
    class RadishException : Exception {
        public static Stack<RadishExceptionEntry> Entries { get; }
        public static int Row { get; set; }
        public static int Col { get; set; }
        public static string FileName { get; set; }
        static RadishException() {
            Row = -1;
            Col = -1;
            FileName = "";
            Entries = new Stack<RadishExceptionEntry>();
        }
        public RadishException(string? message = null, int row = -2, int col = -2, string fileName = "", bool flagged = true) {
            if(message != null) {
                Append(message, row, col, fileName, flagged);
            }
        }
        public static void Append(string message, int row = -2, int col = -2, string fileName = "", bool flagged = true) {
            Entries.Push(new RadishExceptionEntry(message, ((row == -2) ? Row : row), ((col == -2) ? Col : col), ((fileName == "") ? FileName : fileName), flagged));
        }
        public static void Pop() {
            Entries.Pop();
        }
        public static void Print() {
            ConsoleColor current = Console.ForegroundColor;
            string str = "";
            while(Entries.Count > 0) {
                RadishExceptionEntry entry = Entries.Pop();
                if(entry.Flagged) {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                } else {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                Console.WriteLine($"{str}{entry.RMessage}{((entry.Row == -1 || entry.Col == -1) ? "" : $" [row {entry.Row}, column {entry.Col}]")} in {entry.FileName}");
                if(str.Length < (Console.WindowWidth / 2)) {
                    str += "  ";
                }
            }
            Console.ForegroundColor = current;
        }
        public static void Clear() {
            Entries.Clear();
        }


    }
}