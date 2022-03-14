namespace Tools {
    static class Radish {
        public static void Run(string filename, bool verbose = false) {
            CountingReader reader = new CountingReader(filename);
            Operations.ParseScope(reader, verbose).Run();
        }   

        public static void Lex(string filename) {
            CountingReader reader = new CountingReader(filename);
            do {
                LexEntry result = Lexer.Run(reader);
                Console.WriteLine($"{result.Type}: {result.Val}");
            } while(!reader.EndOfStream);
        }

        public static void Parse(string filename) {
            CountingReader reader = new CountingReader(filename);
            Console.WriteLine(Operations.ParseScope(reader, false).Print());
        }
    }
}