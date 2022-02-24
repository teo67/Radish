namespace Tools {
    static class Lexer {
        enum CharTypes {
            letter,
            digit, 
            dot,
            quotes,
            hashtags, 
            whitespace, 
            end,
            operators,
            singlecharoperators, // these cant be chained and are immediately parsed when added
        }
        static Dictionary<char, CharTypes> dict;
        static char dotChar;
        static char quoteChar;
        static char hashChar;
        static Lexer() {
            dict = new Dictionary<char, CharTypes>();
            dotChar = '.';
            quoteChar = '"';
            hashChar = '#';
            string numbers = "0123456789";
            string letters = "abcdefghijklmnopqstuvwxyz";
            for(int i = 0; i < letters.Length; i++) {
                dict.Add(letters[i], CharTypes.letter);
                dict.Add(Char.ToUpper(letters[i]), CharTypes.letter);
            }
            dict.Add('R', CharTypes.letter);
            for(int i = 0; i < numbers.Length; i++) {
                dict.Add(numbers[i], CharTypes.digit);
            }
            string ops = "+-/*=<>|&";
            for(int i = 0; i < ops.Length; i++) {
                dict.Add(ops[i], CharTypes.operators);
            }
            string singlecharops = "(";
            for(int i = 0; i < singlecharops.Length; i++) {
                dict.Add(singlecharops[i], CharTypes.singlecharoperators);
            }
            string enders = ")r";
            for(int i = 0; i < enders.Length; i++) {
                dict.Add(enders[i], CharTypes.end);
            }
            dict.Add(dotChar, CharTypes.dot);
            dict.Add(' ', CharTypes.whitespace);
            dict.Add('\n', CharTypes.whitespace);
            dict.Add(quoteChar, CharTypes.quotes);
            dict.Add(hashChar, CharTypes.hashtags);
        }
        
        private static CharTypes GetCharType(char input) {
            CharTypes returning;
            try {
                returning = dict[input];
            } catch {
                throw new Exception("Unrecognized character!");
            }
            return returning;
        }

        private static TokenTypes GetTokenType(TokenTypes current, CharTypes adding, string currentRaw) { // same = no change
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
                    return (current == TokenTypes.NUMBER && currentRaw.IndexOf(dotChar) == -1) ? TokenTypes.SAME : TokenTypes.OPERATOR; // if youre already in a number with no decimal points yet, consider this a decimal point : otherwise, its punctuation
                case CharTypes.singlecharoperators:
                    return TokenTypes.OPERATOR;
                case CharTypes.operators:
                    return (current == TokenTypes.OPERATOR) ? TokenTypes.SAME : TokenTypes.OPERATOR;
                case CharTypes.end:
                    return TokenTypes.END;
                default:
                    throw new Exception("Something went wrong in the lex phase.");
            }
        }

        public static List<LexEntry> Run(string fileName) {
            StreamReader reader = new StreamReader(fileName);
            List<LexEntry> returning = new List<LexEntry>();
            string currentRaw = "";
            TokenTypes current = TokenTypes.NONE;
            do {
                char read = (char)reader.Read();
                TokenTypes newToken = GetTokenType(current, GetCharType(read), currentRaw);
                if(newToken == TokenTypes.SAME) {
                    currentRaw += read;
                } else {
                    if(current != TokenTypes.COMMENT && current != TokenTypes.NONE) {
                        if(current == TokenTypes.KEYWORD) {
                            TokenTypes type = TokenTypes.KEYWORD;
                            if(currentRaw == "true" || currentRaw == "false") {
                                type = TokenTypes.BOOLEAN;
                            } else if(Operators.O.ContainsKey(currentRaw)) {
                                type = TokenTypes.OPERATOR;
                            }
                            returning.Add(new LexEntry(type, currentRaw));
                        } else {
                            returning.Add(new LexEntry(current, currentRaw));
                        }
                    }
                    current = newToken;
                    currentRaw = Char.ToString(read);
                }
            } while(!reader.EndOfStream);
            if(current != TokenTypes.COMMENT && current != TokenTypes.NONE) {
                returning.Add(new LexEntry(current, currentRaw));
            }
            return returning;
        }
    }
}