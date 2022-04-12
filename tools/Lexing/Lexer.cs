namespace Tools {
    class Lexer {
        enum CharTypes {
            letter,
            digit, 
            dot,
            quotes,
            hashtags, 
            whitespace,
            operators,
            symbols, // these cant be chained and are immediately parsed when added
        }
        private Dictionary<char, CharTypes> dict;
        private char dotChar;
        private char quoteChar;
        private char hashChar;
        private CountingReader reader { get; }
        public Lexer(CountingReader reader) {
            this.reader = reader;
            dict = new Dictionary<char, CharTypes>();
            dotChar = '.';
            quoteChar = '"';
            hashChar = '#';
            string numbers = "0123456789";
            string letters = "abcdefghijklmnopqrstuvwxyz";
            for(int i = 0; i < letters.Length; i++) {
                dict.Add(letters[i], CharTypes.letter);
                dict.Add(Char.ToUpper(letters[i]), CharTypes.letter);
            }
            dict.Add('_', CharTypes.letter);
            for(int i = 0; i < numbers.Length; i++) {
                dict.Add(numbers[i], CharTypes.digit);
            }
            string ops = "+-/*<>|&!=%\\";
            for(int i = 0; i < ops.Length; i++) {
                dict.Add(ops[i], CharTypes.operators);
            }
            string symbols = "(){}[],:";
            for(int i = 0; i < symbols.Length; i++) {
                dict.Add(symbols[i], CharTypes.symbols);
            }
            dict.Add(dotChar, CharTypes.dot);
            dict.Add(' ', CharTypes.whitespace);
            dict.Add('\n', CharTypes.whitespace);
            dict.Add('\r', CharTypes.whitespace);
            dict.Add(quoteChar, CharTypes.quotes);
            dict.Add(hashChar, CharTypes.hashtags);
        }
        
        private CharTypes GetCharType(char input) {
            CharTypes returning;
            try {
                returning = dict[input];
            } catch {
                throw reader.Error($"Unrecognized character: {input}!");
            }
            return returning;
        }

        private TokenTypes GetTokenType(TokenTypes current, CharTypes adding, string currentRaw) { // same = no change
            if(current == TokenTypes.COMMENT && currentRaw.IndexOf(hashChar) == currentRaw.LastIndexOf(hashChar)) { // being in a comment gets first priority
                return TokenTypes.SAME;
            }
            if(current == TokenTypes.STRING && currentRaw.IndexOf(quoteChar) == currentRaw.LastIndexOf(quoteChar)) { // then being in a string
                return TokenTypes.SAME;
            }
            switch(adding) {
                case CharTypes.quotes:
                    return TokenTypes.STRING;
                case CharTypes.hashtags:
                    return TokenTypes.COMMENT;
                case CharTypes.whitespace:
                    return TokenTypes.NONE;
                case CharTypes.letter:
                    return (current == TokenTypes.KEYWORD) ? TokenTypes.SAME : TokenTypes.KEYWORD;
                case CharTypes.digit:
                    return (current == TokenTypes.NUMBER) ? TokenTypes.SAME : TokenTypes.NUMBER;
                case CharTypes.dot:
                    return (current == TokenTypes.NUMBER && currentRaw.IndexOf(dotChar) == -1) ? TokenTypes.SAME : TokenTypes.SYMBOL; // if youre already in a number with no decimal points yet, consider this a decimal point : otherwise, its punctuation
                case CharTypes.symbols:
                    return TokenTypes.SYMBOL;
                case CharTypes.operators:
                    return (current == TokenTypes.OPERATOR) ? TokenTypes.SAME : TokenTypes.OPERATOR;
                default:
                    throw new Exception("Something went wrong in the lex phase.");
            }
        }

        private LexEntry Convert(TokenTypes current, string currentRaw) {
            if(current == TokenTypes.KEYWORD) {
                TokenTypes type = TokenTypes.KEYWORD;
                if(currentRaw == "yes" || currentRaw == "no") {
                    type = TokenTypes.BOOLEAN;
                }
                if(Operations.IsKeyword(currentRaw)) {
                    type = TokenTypes.OPERATOR;
                }
                //Console.WriteLine($"Lexer returning {type}: {currentRaw}");
                return new LexEntry(type, currentRaw);
            }
            if(current == TokenTypes.STRING) {
                return new LexEntry(current, currentRaw.Substring(1, currentRaw.Length - 2));
            }
            //Console.WriteLine($"Lexer returning {current}: {currentRaw}");
            return new LexEntry(current, currentRaw);
        }

        public LexEntry Run() {
            if(reader.EndOfStream) {
                return new LexEntry(TokenTypes.ENDOFFILE, "");
            }
            string currentRaw = "";
            TokenTypes current = TokenTypes.NONE;
            do {
                char read = (char)reader.Peek();
                TokenTypes newToken = GetTokenType(current, GetCharType(read), currentRaw);
                if(newToken == TokenTypes.SAME) {
                    reader.Read();
                    currentRaw += read;
                } else {
                    if(current != TokenTypes.COMMENT && current != TokenTypes.NONE) {
                        return Convert(current, currentRaw);
                    }
                    reader.Read();
                    current = newToken;
                    currentRaw = Char.ToString(read);
                }
            } while(!reader.EndOfStream);
            if(current == TokenTypes.COMMENT || current == TokenTypes.NONE) {
                return new LexEntry(TokenTypes.ENDOFFILE, "");
            }
            return Convert(current, currentRaw);
        }
    }
}