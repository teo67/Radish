namespace Tools {
    class Radish {
        private CountingReader reader { get; }
        public Radish(string filename) {
            reader = new CountingReader(filename);
        }

        public void Run(bool verbose = false) {
            Operations operations = new Operations(reader, verbose);
            operations.ParseScope().Run();
        }   

        public void Lex() {
            Lexer lexer = new Lexer(reader);
            do {
                LexEntry result = lexer.Run();
                Console.WriteLine($"{result.Type}: {result.Val}");
            } while(!reader.EndOfStream);
        }

        public void Parse() {
            Console.WriteLine(new Operations(reader, false).ParseScope().Print());
        }
    }
}