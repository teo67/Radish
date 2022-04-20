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
        public static Stack<RadishExceptionEntry> Entries { get; }
        public static int Row { get; set; }
        public static int Col { get; set; }
        static RadishException() {
            Row = -1;
            Col = -1;
            Entries = new Stack<RadishExceptionEntry>();
        }
        public RadishException(string? message = null, int row = -2, int col = -2) {
            if(message != null) {
                Append(message, row, col);
            }
        }
        public static void Append(string message, int row = -2, int col = -2) {
            Entries.Push(new RadishExceptionEntry(message, ((row == -2) ? Row : row), ((col == -2) ? Col : col)));
        }
        public static void Pop() {
            Entries.Pop();
        }
        public static void Print() {
            ConsoleColor current = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            string str = "";
            while(Entries.Count > 0) {
                RadishExceptionEntry entry = Entries.Pop();
                Console.WriteLine($"{str}{entry.RMessage}{((entry.Row == -1 || entry.Col == -1) ? "" : $" [row {entry.Row}, column {entry.Col}]")}");
                if(str.Length < (Console.WindowWidth / 2)) {
                    str += "  ";
                }
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            }
            Console.ForegroundColor = current;
        }
        public static void Clear() {
            Entries.Clear();
        }
    }
}