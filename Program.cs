class Program {
    private static bool hasOption(string[] choices, string[] args) {
        foreach(string choice in choices) {
            if(args.Contains(choice)) {
                return true;
            }
        }
        return false;
    }
    private static bool hasOption(string choice, string[] args) {
        return args.Contains(choice);
    }
    public static void Main(string[] args) {
        Tools.Radish? radish = null;
        try {
            string first = System.IO.Directory.GetCurrentDirectory();
            string fileName = args.Length > 0 && args[args.Length - 1].EndsWith(".rdsh") ? args[args.Length - 1] : "main.rdsh";
            first = first.Replace('\\', '/') + "/" + fileName; // we use forward slashes
            Tools.RadishException.FileName = first;
            radish = new Tools.Radish(first);
        } catch(Exception e) {
            Console.WriteLine($"Error initiating program: {e.Message}");
        }
        if(radish != null) {
            bool lex = hasOption(new string[] { "l", "lex" }, args);
            bool parse = hasOption(new string[] { "p", "parse" }, args);
            bool minify = hasOption("minify", args);
            if(lex) {
                radish.Lex();
            } else if(parse) {
                radish.Parse();
            } else {
                bool verbose = hasOption(new string[] { "v", "verbose" }, args);
                bool nolib = hasOption("nolib", args);
                radish.Run(verbose, !nolib);
            }
        }
    }
}