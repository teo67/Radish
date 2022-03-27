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
                "if", "elseif", "else", "while", "loop", "val", "function", "out", "cancel", "continue"
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

            stack.Push();
            stack.Head.Val.Add(
                new Values.Variable(
                    "output", 
                    new Values.FunctionLiteral(
                        stack, 
                        new List<string>() { "input" }, 
                        new Operators.ExpressionSeparator(
                            new List<IOperator>() {
                                new Operators.ReturnType("out", 
                                    new Operators.Output(
                                        new Operators.Reference(stack, "input")
                                    )
                                )
                            }
                        )
                    )
                )
            ); // we're manually adding output to the stack for now, eventually it will become an import
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
                } else if(read.Type == TokenTypes.OPERATOR && read.Val == "loop") {
                    RequireSymbol("(");
                    Operators.ListSeparator list = ParseList();
                    RequireSymbol(")");
                    RequireSymbol("{");
                    IOperator body = ParseScope();
                    RequireSymbol("}");
                    returning.AddValue(new Operators.Loop(stack, list, body));
                } else if(read.Type == TokenTypes.OPERATOR && (read.Val == "out" || read.Val == "cancel" || read.Val == "continue")) {
                    Print("parsing return statement");
                    LexEntry next = Read();
                    if(next.Type == TokenTypes.SYMBOL && next.Val == "r") {
                        Print("parsing empty return");
                        returning.AddValue(new Operators.ReturnType(read.Val));
                    } else {
                        Stored = next;
                        IOperator carrying = ParseExpression();
                        Print("parsing endline");
                        RequireSymbol("r");
                        returning.AddValue(new Operators.ReturnType(read.Val, carrying));
                    }
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

        private T ParseLi<T>(Func<T> init, Action<T> onEach) {
            Print("begin list");
            T returning = init();
            LexEntry read = Read();
            if(read.Type == TokenTypes.SYMBOL && (read.Val == "]" || read.Val == ")")) { // empty list
                Stored = read;
                return returning;
            }
            while(true) { 
                Print("parsing list element");
                Stored = read;
                onEach(returning);
                
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

        private Operators.ListSeparator ParseList() {
            return ParseLi<Operators.ListSeparator>(() => {
                return new Operators.ListSeparator();
            }, (Operators.ListSeparator returning) => {
                returning.AddValue(ParseExpression());
            });
        }

        private List<string> ParseArgs() {
            return ParseLi<List<string>>(() => {
                return new List<string>();
            }, (List<string> returning) => {
                LexEntry key = Read();
                if(key.Type != TokenTypes.KEYWORD) {
                    Error("Expecting a function parameter!");
                }
                returning.Add(key.Val);
            });
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
            IOperator current = ParseNegatives();
            LexEntry next = Read();
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "*") {
                        Print("parsing multiply");
                        IOperator after = ParseNegatives();
                        current = new Operators.Multiply(current, after);
                        return true;
                    }
                    if(next.Val == "/") {
                        Print("parsing divide");
                        IOperator after = ParseNegatives();
                        current = new Operators.Divide(current, after);
                        return true;
                    }
                    if(next.Val == "%") {
                        Print("parsing modulo");
                        IOperator after = ParseNegatives();
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

        private IOperator ParseNegatives() {
            LexEntry returned = Read();
            if(returned.Type == TokenTypes.OPERATOR) {
                if(returned.Val == "-") {
                    return new Operators.Multiply(new Operators.Number(-1.0), ParseNegatives());
                }
                if(returned.Val == "!") {
                    return new Operators.Invert(ParseNegatives());
                }
            }
            Stored = returned;
            return ParseCalls();
        }
        
        private IOperator ParseCalls() {
            IOperator current = ParseLowest();
            LexEntry next = Read();
            Func<bool> checkCheck = (() => {
                if(next.Val == "(") {
                    Print("parsing function call");
                    IOperator args = ParseList();
                    RequireSymbol(")");
                    current = new Operators.FunctionCall(current, args);
                    return true;
                }
                if(next.Val == "[") {
                    Print("parsing array accessor");
                    IOperator exp = ParseExpression();
                    RequireSymbol("]");
                    current = new Operators.ArrayGet(current, exp);
                    return true;
                }
                return false;
            });
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.SYMBOL) {
                    while(checkCheck()) {
                        next = Read();
                    }
                    if(next.Val == ".") {
                        Print("parsing accessor");
                        // etc
                        // ParseLowest();
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
                    List<string> args = ParseArgs();
                    RequireSymbol(")");
                    RequireSymbol("{");
                    Operators.ExpressionSeparator body = ParseScope();
                    RequireSymbol("}");
                    Operators.FunctionDefinition def = new Operators.FunctionDefinition(stack, args, body);
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
                Print("parsing variable");
                return new Operators.Reference(stack, returned.Val);
            } else if(returned.Type == TokenTypes.SYMBOL) {
                if(returned.Val == "(") {
                    Print("parsing opening paren.");
                    Print("parsing expression");
                    IOperator returning = ParseExpression();
                    Print("parsing closing paren.");
                    RequireSymbol(")");
                    return returning;
                } 
                if(returned.Val == "[") {
                    Print("parsing opening square brackets");
                    Operators.ListSeparator returning = ParseList();
                    Print("parsing closing square brackets");
                    RequireSymbol("]");
                    return returning;
                }
            }
            throw Error($"Could not parse value: {returned.Type}: {returned.Val} !");
        }
    }
}