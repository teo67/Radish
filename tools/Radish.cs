namespace Tools {
    class Radish {
        private CountingReader reader { get; }
        public Radish(string filename) {
            reader = new CountingReader(filename);
        }

        public void Run(bool verbose = false) {
            try {
                Operations operations = new Operations(reader, verbose);
                operations.ParseScope().Run();
            } catch(RadishException e) {
                Console.WriteLine($"{e.RMessage} [row {e.Row}, column {e.Col}]");
            }
        }   

        public void Lex() {
            try {
                Lexer lexer = new Lexer(reader);
                do {
                    LexEntry result = lexer.Run();
                    Console.WriteLine($"{result.Type}: {result.Val}");
                } while(!reader.EndOfStream);
            } catch(RadishException e) {
                Console.WriteLine($"{e.RMessage} [row {e.Row}, column {e.Col}]");
            }
        }

        public void Parse() {
            try {
                Console.WriteLine(new Operations(reader, false).ParseScope().Print());
            } catch(RadishException e) {
                Console.WriteLine($"{e.RMessage} [row {e.Row}, column {e.Col}]");
            }
        }
    }
}