class Program {
    public static void Main(string[] args) {
        //Console.WriteLine("Hello, world!");
        Tools.Radish? radish = null;
        string? second = null;
        try {
            string first = System.IO.Directory.GetCurrentDirectory();
            first = first.Replace('\\', '/'); // we use forward slashes
            second = (args.Length > 0 ? args[0] : "run");
            Tools.RadishException.Append("in main.rdsh", -1, -1);
            radish = new Tools.Radish(first + "/main.rdsh");
        } catch(Exception e) {
            Console.WriteLine($"Error initiating program: {e.Message}");
        }
        if(radish != null && second != null) {
            switch(second) {
                case "l":
                case "lex":
                    radish.Lex();
                    break;
                case "p":
                case "parse":
                    radish.Parse();
                    break;
                case "v":
                case "verbose":
                    radish.Run(true);
                    break;
                case "r":
                case "run":
                default:
                    radish.Run();
                    break;
            }
        }
    }
}
