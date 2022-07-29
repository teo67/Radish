namespace Tools {
    class Lexer {
        enum CharTypes {
            letter,
            digit, 
            dot,
            quotes,
            hashtags, 
            semis,
            whitespace,
            operators,
            symbols, // these cant be chained and are immediately parsed when added
        }
        private Dictionary<char, CharTypes> dict;
        private char dotChar;
        private char hashChar;
        private char semi;
        private IReader reader { get; }
        public static Dictionary<char, char> backslashes;
        static Lexer() {
            backslashes = new Dictionary<char, char>() {
                { 'n', '\n' },
                { 't', '\t' }
            };
        }
        public Lexer(IReader reader) {
            this.reader = reader;
            dict = new Dictionary<char, CharTypes>();
            dotChar = '.';
            hashChar = '#';
            semi = ';';
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
            string ops = "+-/*<>|&!=%\\^~?";
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
            dict.Add('\xa0', CharTypes.whitespace);
            dict.Add('\t', CharTypes.whitespace);
            dict.Add('"', CharTypes.quotes);
            dict.Add('\'', CharTypes.quotes);
            dict.Add(hashChar, CharTypes.hashtags);
            dict.Add(semi, CharTypes.semis);
        }
        
        private CharTypes GetCharType(char input) {
            try {
                return dict[input];
            } catch {
                return CharTypes.letter;
            }
        }

        private TokenTypes GetTokenType(TokenTypes current, CharTypes adding, string currentRaw) { // same = no change
            if(current == TokenTypes.COMMENT && (currentRaw.Length == 1 || currentRaw[currentRaw.Length - 1] != hashChar)) { // being in a comment gets first priority
                return TokenTypes.SAME;
            }
            if(current == TokenTypes.SEMIS && (currentRaw.Length == 1 || currentRaw[currentRaw.Length - 1] != semi)) { // being in a comment gets first priority
                return TokenTypes.SAME;
            }
            if(current == TokenTypes.STRING && (currentRaw.Length == 1 || (currentRaw[currentRaw.Length - 1] != '"' && currentRaw[currentRaw.Length - 1] != '\''))) { // then being in a string
                return TokenTypes.SAME;
            }
            switch(adding) {
                case CharTypes.quotes:
                    return TokenTypes.STRING;
                case CharTypes.hashtags:
                    return TokenTypes.COMMENT;
                case CharTypes.semis:
                    return TokenTypes.SEMIS;
                case CharTypes.whitespace:
                    return TokenTypes.NONE;
                case CharTypes.letter:
                    return (current == TokenTypes.KEYWORD) ? TokenTypes.SAME : TokenTypes.KEYWORD;
                case CharTypes.digit:
                    return (current == TokenTypes.NUMBER || current == TokenTypes.KEYWORD) ? TokenTypes.SAME : TokenTypes.NUMBER;
                case CharTypes.dot:
                    return (current == TokenTypes.NUMBER && currentRaw.IndexOf(dotChar) == -1) ? TokenTypes.SAME : TokenTypes.SYMBOL; // if youre already in a number with no decimal points yet, consider this a decimal point : otherwise, its punctuation
                case CharTypes.symbols:
                    return TokenTypes.SYMBOL;
                case CharTypes.operators:
                    return (current == TokenTypes.OPERATOR) ? TokenTypes.SAME : TokenTypes.OPERATOR;
                default:
                    throw new RadishException("Something went wrong in the lex phase.");
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
            return new LexEntry(current, currentRaw);
        }

        public LexEntry Run() {
            if(reader.EndOfStream) {
                return new LexEntry(TokenTypes.ENDOFFILE, "");
            }
            string currentRaw = "";
            TokenTypes current = TokenTypes.NONE;
            bool skip = false;
            do {
                char read = (char)reader.Peek();
                if(read == '\\' && current == TokenTypes.STRING) {
                    reader.Read();
                    char next = reader.Peek();
                    reader.Read();
                    if(backslashes.ContainsKey(next)) {
                        currentRaw += backslashes[next];
                    } else {
                        currentRaw += next;
                    }
                    skip = true;
                } else {
                    TokenTypes newToken = GetTokenType(current, GetCharType(read), currentRaw);
                    if(skip || newToken == TokenTypes.SAME) {
                        skip = false;
                        reader.Read();
                        currentRaw += read;
                    } else {
                        if(current != TokenTypes.COMMENT && current != TokenTypes.NONE && current != TokenTypes.SEMIS) {
                            return Convert(current, currentRaw);
                        }
                        reader.Read();
                        current = newToken;
                        currentRaw = Char.ToString(read);
                    }
                }
            } while(!reader.EndOfStream);
            if(current == TokenTypes.COMMENT || current == TokenTypes.NONE || current == TokenTypes.SEMIS) {
                return new LexEntry(TokenTypes.ENDOFFILE, "");
            }
            return Convert(current, currentRaw);
        }
    }
}