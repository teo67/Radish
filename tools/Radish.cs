namespace Tools {
    class Radish {
        private CountingReader reader { get; }
        public Radish(string filename) {
            reader = new CountingReader(filename);
        }

        public void Minify() {
            bool[] options = new bool[7];
            string[] prompts = new string[7] {
                "use operators like +=, -=, *= when possible",
                "use increment and decrement operators when possible", "shorten keywords such as 'plant' to their alternates whenever possible",
                "shorten function declarations when they have no parameters and/or only one operation in their function body",
                "reassign variable names to shorten length", "reassign property names to shorten length", "include imported files within the minified file"
            };
            for(int i = 0; i < 7; i++) {
                Console.Write($"[y/n] Would you like to {prompts[i]}? ");
                string? res = Console.ReadLine();
                options[i] = !(res != null && (res.ToLower() == "n" || res.ToLower() == "no"));
            }
            MinifyOptions minifyOptions = new MinifyOptions(options);
            Console.Write("Please enter the path to the file to be written to (default: minified.rdsh): ");
            string? _path = Console.ReadLine();
            string path = (_path == null || _path.Length == 0) ? "minified.rdsh" : _path;
            Console.WriteLine("Working...");
            try {
                Minifier minifier = new Minifier(reader, new Librarian(false));
                minifier.ParseScope();
                File.WriteAllText(path, minifier.Output);
                Console.WriteLine($"The file has been minified to {path}!");
            } catch(RadishException) {
                RadishException.Print();
            } catch(Exception e) {
                Console.WriteLine("Uh oh, a C# error occurred within Radish. Here's the raw output:");
                Console.WriteLine(e);
            }
        }

        public void Run(bool verbose = false, bool uselib = true) {
            try {
                Librarian librarian = new Librarian(uselib);
                Operations operations = new Operations(reader, verbose, false, librarian);
                operations.ParseScope().Run(operations.stack);
            } catch(RadishException) {
                RadishException.Print();
            } catch(Exception e) {
                Console.WriteLine("Uh oh, a C# error occurred within Radish. Here's the raw output:");
                Console.WriteLine(e);
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