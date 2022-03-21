 /* priority list    
        0: scope - entire script, if statement, for loop etc
        1: expression - an entire line of code (does not include endline char, that gets checked for in layer 0 if necessary)
            examples: literally just the = operator
        2: boolean operators: &&, ||
        3: more boolean operators: ==, <=, >=, <, >
        4: term operators: +, -
        5: factor operators: *, /, %
        6: directly before operators: - (as in -a), !, let (as in let varName = ...)

        every layer except for 0 and 6 consists of at least one of the next layer with operators of that layer in between
        6 consists of either just a literal, just a keyword, or an operator followed by either of those (literal includes array, going to make some kind of parse list function)
        0 consists of at least zero of either operator(list) { parse(0) } or an expression, where expressions must be followed by r

        sample case:
        let test = 3r
        if(test == 3) {
            print(test + 1)r
        }

        0 -> find 1, 2, 3, 4, 5, 6 -> find let operator followed by keyword -> resolve until 1 -> find = -> find 2, 3, 4, 5, 6 -> find literal -> resolve until 0 -> find r
        -> find if( -> parse list -> find 1, 2, 3 -> find == -> find 4, 5, 6 -> find literal -> resolve until 1, resolve list -> find ) { -> 0
        -> find 1, 2, 3, 4, 5, 6 -> find print keyword followed by ( -> find 1, 2, 3, 4 -> find keyword -> find + -> find 5, 6 -> find literal -> resolve through 1 -> find )
        -> resolve until 0 -> find r -> resolve 0 -> find } -> resolve 0
*/
namespace Tools {
    class Operations {
        private static List<string> OpKeywords { get; }
        private LexEntry? Stored { get; set; }
        private int PrevRow { get; set; }
        private int PrevCol { get; set; }
        private bool verbose { get; }
        private CountingReader reader { get; }
        private Lexer lexer { get; }
        private Stack stack { get; }
        static Operations() {
            OpKeywords = new List<string>() {
                // keywords that should be parsed as operators
                "if", "elseif", "else", "while", "whileval", "val", "function"
            };
        }
        public Operations(CountingReader reader, bool verbose) {
            this.reader = reader;
            this.verbose = verbose;
            this.lexer = new Lexer(reader);
            this.stack = new Stack(new StackNode(new List<Values.Variable>()));
            Stored = null;
            PrevRow = -1;
            PrevCol = -1;
        }

        public static bool IsKeyword(string input) {
            return OpKeywords.Contains(input);
        }

        private void RequireSymbol(string input) {
            Print($"requiring {input}");
            LexEntry next = Read();
            if(!(next.Type == TokenTypes.SYMBOL && next.Val == input)) {
                throw Error($"Missing expected symbol: {input}");
            }
        }

        private void Print(string input) {
            if(verbose) {
                Console.WriteLine(input);
            }
        }

        private Exception Error(string input) {
            if(Stored == null) {
                return reader.Error(input);
            }
            return reader.Error(input, PrevRow, PrevCol);        
        }

        private LexEntry Read() {
            //Print("reading");
            if(Stored != null) {
                LexEntry saved = Stored;
                Stored = null;
                return saved;
            }
            PrevCol = reader.col;
            PrevRow = reader.row;
            return lexer.Run();
        }

        public Operators.ExpressionSeparator ParseScope() {
            Print("begin scope");
            Operators.ExpressionSeparator returning = new Operators.ExpressionSeparator();
            LexEntry read = Read();
            while(read.Type != TokenTypes.ENDOFFILE && !(read.Type == TokenTypes.SYMBOL && read.Val == "}")) {
                if(read.Type == TokenTypes.OPERATOR && read.Val == "if") {
                    Stored = read;
                    returning.AddValue(ParseIfs());
                } else if(read.Type == TokenTypes.OPERATOR && read.Val == "while") {
                    RequireSymbol("(");
                    IOperator exp = ParseExpression();
                    RequireSymbol(")");
                    RequireSymbol("{");
                    IOperator scope = ParseScope();
                    RequireSymbol("}");
                    returning.AddValue(new Operators.While(stack, exp, scope));
                } else if(read.Type == TokenTypes.OPERATOR && read.Val == "whileval") {
                    RequireSymbol("(");
                    Operators.ListSeparator list = ParseList();
                    RequireSymbol(")");
                    RequireSymbol("{");
                    IOperator body = ParseScope();
                    RequireSymbol("}");
                    returning.AddValue(new Operators.WhileVal(stack, list, body));
                } else {
                    Print("parsing expression");
                    Stored = read;
                    returning.AddValue(ParseExpression());
                    Print("parsing endline (r)");
                    RequireSymbol("r");
                }
                read = Read();
            }
            Stored = read;
            return returning;
        }

        private Operators.IfChain ParseIfs() {
            Print("begin if chain");
            Operators.IfChain returning = new Operators.IfChain();
            LexEntry IF = Read();
            if(!(IF.Type == TokenTypes.OPERATOR && IF.Val == "if")) {
                throw Error("Expecting if statement!");
            }
            returning.AddValue(ParseIf());
            LexEntry read = Read();
            while(read.Type == TokenTypes.OPERATOR && read.Val == "elseif") {
                returning.AddValue(ParseIf());
                read = Read();
            }
            if(read.Type == TokenTypes.OPERATOR && read.Val == "else") {
                RequireSymbol("{");
                IOperator scope = ParseScope();
                RequireSymbol("}");
                returning.AddValue(new Operators.If(stack, new Operators.Boolean(true), scope));
            } else {
                Stored = read;
            }
            return returning;
        }

        private Operators.If ParseIf() {
            RequireSymbol("(");
            IOperator li = ParseExpression();
            RequireSymbol(")");
            RequireSymbol("{");
            IOperator scope = ParseScope();
            RequireSymbol("}");
            return new Operators.If(stack, li, scope);
        }

        private Operators.ListSeparator ParseList() {
            Print("begin list");
            Operators.ListSeparator returning = new Operators.ListSeparator();
            LexEntry read = Read();
            if(read.Type == TokenTypes.SYMBOL && (read.Val == "]" || read.Val == ")")) { // empty list
                Stored = read;
                return returning;
            }
            while(true) { 
                Print("parsing list element");
                Stored = read;
                returning.AddValue(ParseExpression());
                LexEntry next = Read();

                if(!(next.Type == TokenTypes.SYMBOL && next.Val == ",")) {
                    Stored = next;
                    break;
                } else {
                    Print("found comma");
                }

                read = Read();
            }
            return returning;
        }

        private IOperator ParseExpression() {
            Print("begin expression");
            IOperator current = ParseCombiners();
            LexEntry next = Read();
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "=") {
                        Print("parsing variable assignment");
                        IOperator after = ParseCombiners();
                        current = new Operators.Assignment(current, after);
                        return true;
                    }
                }
                Stored = next; // cancel viewing
                return false;
            });
            while(check()) {
                next = Read();
            }
            return current;
        }

        private IOperator ParseCombiners() {
            Print("begin combiners");
            IOperator current = ParseComparators();
            LexEntry next = Read();
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "&&") {
                        Print("parsing and");
                        IOperator after = ParseComparators();
                        current = new Operators.And(current, after);
                        return true;
                    }
                    if(next.Val == "||") {
                        Print("parsing or");
                        IOperator after = ParseComparators();
                        current = new Operators.Or(current, after);
                        return true;
                    }
                }
                Stored = next; // cancel viewing
                return false;
            });
            while(check()) {
                next = Read();
            }
            return current;
        }

        private IOperator ParseComparators() {
            Print("begin comparators");
            IOperator current = ParseTerms();
            LexEntry next = Read();
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "==") {
                        Print("parsing double equals");
                        IOperator after = ParseTerms();
                        current = new Operators.EqualsEquals(current, after);
                        return true;
                    }
                    if(next.Val == ">=") {
                        Print("parsing more than equals");
                        IOperator after = ParseTerms();
                        current = new Operators.MoreThanOrEquals(current, after);
                        return true;
                    }
                    if(next.Val == "<=") {
                        Print("parsing less than equals");
                        IOperator after = ParseTerms();
                        current = new Operators.LessThanOrEquals(current, after);
                        return true;
                    }
                    if(next.Val == ">") {
                        Print("parsing more than");
                        IOperator after = ParseTerms();
                        current = new Operators.MoreThan(current, after);
                        return true;
                    }
                    if(next.Val == "<") {
                        Print("parsing less than");
                        IOperator after = ParseTerms();
                        current = new Operators.LessThan(current, after);
                        return true;
                    }
                }
                Stored = next; // cancel viewing
                return false;
            });
            while(check()) {
                next = Read();
            }
            return current;
        }

        private IOperator ParseTerms() {
            Print("begin terms");
            IOperator current = ParseFactors();
            LexEntry next = Read();
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "+") {
                        Print("parsing plus");
                        IOperator after = ParseFactors();
                        current = new Operators.Add(current, after);
                        return true;
                    }
                    if(next.Val == "-") {
                        Print("parsing minus");
                        IOperator after = ParseFactors();
                        current = new Operators.Subtract(current, after);
                        return true;
                    }
                }
                Stored = next; // cancel viewing
                return false;
            });
            while(check()) {
                next = Read();
            }
            return current;
        }

        private IOperator ParseFactors() {
            Print("begin factors");
            IOperator current = ParseLowest();
            LexEntry next = Read();
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "*") {
                        Print("parsing multiply");
                        IOperator after = ParseLowest();
                        current = new Operators.Multiply(current, after);
                        return true;
                    }
                    if(next.Val == "/") {
                        Print("parsing divide");
                        IOperator after = ParseLowest();
                        current = new Operators.Divide(current, after);
                        return true;
                    }
                    if(next.Val == "%") {
                        Print("parsing modulo");
                        IOperator after = ParseLowest();
                        current = new Operators.Modulo(current, after);
                        return true;
                    }
                }
                Stored = next; // cancel viewing
                return false;
            });
            while(check()) {
                next = Read();
            }
            return current;
        }
        
        private IOperator ParseLowest() {
            Print("begin lowest");
            LexEntry returned = Read();
            if(returned.Type == TokenTypes.OPERATOR) {
                if(returned.Val == "-") {
                    Print("parsing negative");
                    return new Operators.Multiply(new Operators.Number(-1.0), ParseLowest());
                }
                if(returned.Val == "!") {
                    Print("parsing not");
                    return new Operators.Invert(ParseLowest());
                }
                if(returned.Val == "val") {
                    Print("parsing variable definition");
                    LexEntry next = Read();
                    if(next.Type == TokenTypes.KEYWORD) {
                        return new Operators.Declaration(stack, next.Val);
                    }
                    throw Error("No variable name received!");
                }
                if(returned.Val == "function") {
                    Print("parsing function definition");
                    RequireSymbol("(");
                    Operators.ListSeparator args = ParseList();
                    RequireSymbol(")");
                    RequireSymbol("{");
                    Operators.ExpressionSeparator body = ParseScope();
                    RequireSymbol("}");
                    Operators.FunctionDefinition def = new Operators.FunctionDefinition(stack, args, body);
                    LexEntry next = Read();
                    if(next.Type == TokenTypes.SYMBOL && next.Val == "(") {
                        Print("parsing literal function call");
                        Operators.FunctionCall call = new Operators.FunctionCall(def, ParseList());
                        RequireSymbol(")");
                        return call;
                    }
                    Stored = next;
                    return def;
                }
            } else if(returned.Type == TokenTypes.STRING) {
                Print("parsing string");
                return new Operators.String(returned.Val);
            } else if(returned.Type == TokenTypes.NUMBER) {
                Print("parsing number");
                return new Operators.Number(Double.Parse(returned.Val));
            } else if(returned.Type == TokenTypes.BOOLEAN) {
                Print("parsing boolean");
                return new Operators.Boolean(returned.Val == "yes");
            } else if(returned.Type == TokenTypes.KEYWORD) {
                Print("parsing variable or function call");
                LexEntry next = Read();
                if(next.Type == TokenTypes.SYMBOL && next.Val == "(") {
                    Print("parsing function call");
                    IOperator returning;
                    if(returned.Val == "output") {
                        returning = new Operators.Output(ParseExpression()); // still working on objects and default methods, so this is an override
                    } else {
                        returning = new Operators.FunctionCall(new Operators.Reference(stack, returned.Val), ParseList());
                    }
                    RequireSymbol(")");
                    return returning;
                } else {
                    Print("parsing variable");
                    Stored = next;
                    return new Operators.Reference(stack, returned.Val);
                }
            } else if(returned.Type == TokenTypes.SYMBOL) {
                if(returned.Val == "(") {
                    Print("parsing opening paren.");
                    Print("parsing expression");
                    IOperator returning = ParseExpression();
                    Print("parsing closing paren.");
                    RequireSymbol(")");
                    return returning;
                }
            }
            throw Error($"Could not parse value: {returned.Type}: {returned.Val} !");
        }
    }
}