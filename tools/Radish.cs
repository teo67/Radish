namespace Tools {
    static class Radish {
        public static void Run(string filename) {
            StreamReader reader = new StreamReader(filename);
            do {
                Console.WriteLine(Lexer.Run(reader));
            } while(!reader.EndOfStream);
            Operators.Operators.Test();
        }   
    }
}