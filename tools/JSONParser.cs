namespace Tools {
    class JSONParser {
        private string Input { get; }
        private int Index { get; set; }
        public JSONParser(string input) {
            this.Input = input;
            Index = 0;
        }
        private void Clear() {
            string whitespace = " \n\t\xa0";
            while(Index < Input.Length && whitespace.Contains(Input[Index])) {
                Index++;
            }
        }
        private char Read() {
            if(Index >= Input.Length) {
                throw new RadishException("Unexpected end of JSON input!");
            }
            Index++;
            return Input[Index - 1];
        }
        public IValue Parse() {
            Clear();
            string read = "" + Read();
            string nums = "0123456789";
            if(nums.Contains(read)) {
                string parsing = read;
                while(Index < Input.Length) {
                    char newRead = Read();
                    if(nums.Contains(newRead) || newRead == '.' && (!parsing.Contains("."))) {
                        parsing += newRead;
                    } else {
                        Index--;
                        break;
                    }
                }
                return new Values.NumberLiteral(Double.Parse(parsing));
            } else if(read == "\"") {
                string parsing = "";
                while(true) {
                    char newRead = Read();
                    if(newRead == '\\') {
                        char next = Read();
                        if(Lexer.backslashes.ContainsKey(next)) {
                            parsing += Lexer.backslashes[next];
                        } else {
                            parsing += next;
                        }
                    } else if(newRead == '"') {
                        return new Values.StringLiteral(parsing);
                    } else {
                        parsing += newRead;
                    }
                }
            } else if(read == "[") {
                Clear();
                char next = Read();
                Dictionary<string, Values.Variable> returning = new Dictionary<string, Values.Variable>();
                int count = 0;
                while(next != ']') {
                    Index--;
                    returning.Add($"{count}", new Values.Variable(Parse()));
                    Clear();
                    next = Read();
                    if(next == ',') {
                        Clear();
                        next = Read();
                    }
                    count++;
                }
                return new Values.ObjectLiteral(returning, useArrayProto: true);
            } else if(read == "{") {
                Clear();
                char next = Read();
                Dictionary<string, Values.Variable> returning = new Dictionary<string, Values.Variable>();
                while(next != '}') {
                    Index--;
                    IValue first = Parse();
                    Clear();
                    if(Index >= Input.Length || Input[Index] != ':') {
                        throw new RadishException("Expecting a colon!");
                    }
                    Index++;
                    returning.Add(first.String, new Values.Variable(Parse()));
                    Clear();
                    next = Read();
                    if(next == ',') {
                        Clear();
                        next = Read();
                    }
                }
                return new Values.ObjectLiteral(returning, useProto: true);
            } else {
                string parsing = "" + read;
                string[] valid = {"null", "true", "false"};
                while(!valid.Contains(parsing)) {
                    parsing += Read();
                }
                switch(parsing) {
                    case "null":
                        return new Values.NoneLiteral();
                    case "true":
                        return new Values.BooleanLiteral(true);
                    case "false":
                        return new Values.BooleanLiteral(false);
                }
                throw new RadishException("True, false, or null value expected!");
            }
         }
    }
}