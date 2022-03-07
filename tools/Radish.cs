namespace Tools {
    static class Radish {
        public static void Run(string filename) {
            StreamReader reader = new StreamReader(filename);
            Operations.ParseScope(reader).Run();
        }   

        public static void Lex(string filename) {
            StreamReader reader = new StreamReader(filename);
            do {
                LexEntry result = Lexer.Run(reader);
                Console.WriteLine($"{result.Type}: {result.Val}");
            } while(!reader.EndOfStream);
        }

        public static void Parse(string filename) {
            StreamReader reader = new StreamReader(filename);
            Console.WriteLine(Operations.ParseScope(reader).Print());
        }
    }
}