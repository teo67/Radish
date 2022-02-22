class Program {
    public static void Main() {
        List<Tools.LexEntry> entries = Tools.Lexer.Run(System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"\main.radish"));
        foreach(Tools.LexEntry entry in entries) {
            Console.WriteLine($"{entry.Type}: {entry.Val}");
        }
    }
}
