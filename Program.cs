class Program {
    public static void Main(string[] args) {
        if(args.Length < 1) {
            throw new Exception("Missing argument!");
        }
        int index = args[0].IndexOf('~');
        if(index == -1 || index == 0) {
            throw new Exception("Something went wrong, try again later.");
        }
        string first = args[0].Substring(0, index);
        if(args[0].Length < index + 2) {
            throw new Exception("No argument provided!");
        }
        string second = args[0].Substring(index + 1);
        Tools.Radish radish = new Tools.Radish(first);
        switch(second) {
            case "r":
            case "run":
                radish.Run();
                break;
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
            default:
                throw new Exception($"Invalid argument: {second}!");
        }
    }
}
