namespace Tools {
    class Radish {
        private CountingReader reader { get; }
        public Radish(string filename) {
            reader = new CountingReader(filename);
        }

        public void Run(bool verbose = false, bool uselib = true) {
            try {
                Librarian librarian = new Librarian(uselib);
                Operations operations = new Operations(reader, verbose, false, librarian);
                operations.ParseScope().Run(operations.stack);
            } catch {
                RadishException.Print();
            }
        }   

        public void Lex() {
            try {
                Lexer lexer = new Lexer(reader);
                do {
                    LexEntry result = lexer.Run();
                    Console.WriteLine($"{result.Type}: {result.Val}");
                } while(!reader.EndOfStream);
            } catch {
                RadishException.Print();
            }
        }

        public void Parse() {
            try {
                Librarian librarian = new Librarian(false);
                Console.WriteLine(new Operations(reader, true, false, librarian).ParseScope().Print());
            } catch {
                RadishException.Print();
            }
        }
    }
}