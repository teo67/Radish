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
        public Stack stack { get; }
        private Prototypes prototypes { get; }
        static Operations() {
            OpKeywords = new List<string>() {
                // keywords that should be parsed as operators
                "if", "elseif", "else", 
                "while", "for", 
                "dig", "d", "tool", "t", "plant", "p",
                "harvest", "h", "cancel", "continue", "end",
                "new", "null", "class",
                "public", "private", "protected", "static",
                "try", "catch", "throw", "import", "all", "pass"
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
            prototypes = new Prototypes(this.stack);
            stack.Push();
        }

        public static bool IsKeyword(string input) {
            return OpKeywords.Contains(input);
        }

        private void RequireSymbol(string input) {
            Print($"requiring {input}");
            LexEntry next = Read();
            if(!(next.Type == TokenTypes.SYMBOL && next.Val == input)) {
                throw Error($"Error: expected symbol: {input}");
            }
        }

        private void Print(string input) {
            if(verbose) {
                Console.WriteLine($"{input} ({Row}, {Col})");
            }
        }

        private Exception Error(string input) {
            if(Stored == null) {
                return reader.Error(input);
            }
            return reader.Error(input, PrevRow, PrevCol);        
        }

        private int Row {
            get {
                if(Stored == null) {
                    return reader.row;
                }
                return PrevRow;
            }
        }

        private int Col {
            get {
                if(Stored == null) {
                    return reader.col;
                }
                return PrevCol;
            }
        }

        private LexEntry Read() {
            //Print("reading");
            if(Stored != null) {
                LexEntry saved = Stored;
                Stored = null;
                //Print(saved.Val);
                return saved;
            }
            PrevCol = reader.col;
            PrevRow = reader.row;
            LexEntry ran = lexer.Run();
            //Print(ran.Val);
            return ran;
        }

        public Operators.ExpressionSeparator ParseScope() {
            Print("begin scope");
            Operators.ExpressionSeparator returning = new Operators.ExpressionSeparator(Row, Col);
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
                    returning.AddValue(new Operators.While(stack, exp, scope, Row, Col));
                } else if(read.Type == TokenTypes.OPERATOR && read.Val == "for") {
                    RequireSymbol("(");
                    Operators.ListSeparator list = ParseList();
                    RequireSymbol(")");
                    RequireSymbol("{");
                    IOperator body = ParseScope();
                    RequireSymbol("}");
                    returning.AddValue(new Operators.Loop(stack, list, body, Row, Col));
                } else if(read.Type == TokenTypes.OPERATOR && (read.Val == "cancel" || read.Val == "continue" || read.Val == "end")) {
                    Print("parsing end/c/c statement");
                    returning.AddValue(new Operators.ReturnType(read.Val, Row, Col));
                } else if(read.Type == TokenTypes.OPERATOR && (read.Val == "harvest" || read.Val == "h")) {
                    Print("parsing out statement");
                    IOperator carrying = ParseExpression();
                    returning.AddValue(new Operators.ReturnType("harvest", Row, Col, carrying));
                } else if(read.Type == TokenTypes.OPERATOR && read.Val == "try") {
                    Print("parsing try/catch");
                    RequireSymbol("{");
                    IOperator tryScope = ParseScope();
                    RequireSymbol("}");
                    LexEntry next = Read();
                    if(next.Type == TokenTypes.OPERATOR && next.Val == "catch") {
                        RequireSymbol("{");
                        IOperator catchScope = ParseScope();
                        RequireSymbol("}");
                        returning.AddValue(new Operators.TryCatch(tryScope, catchScope, stack, Row, Col));
                    } else {
                        throw Error("Expecting catch phrase after try {}");
                    }
                } else {
                    Print("parsing expression");
                    Stored = read;
                    returning.AddValue(ParseExpression());
                }
                read = Read();
            }
            Stored = read;
            return returning;
        }

        private Operators.IfChain ParseIfs() {
            Print("begin if chain");
            Operators.IfChain returning = new Operators.IfChain(Row, Col);
            LexEntry IF = Read();
            if(!(IF.Type == TokenTypes.OPERATOR && IF.Val == "if")) {
                throw new RadishException("Expecting if statement!");
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
                returning.AddValue(new Operators.If(stack, new Operators.Boolean(true, stack, Row, Col), scope, Row, Col));
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
            return new Operators.If(stack, li, scope, Row, Col);
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
                return new Operators.ListSeparator(stack, Row, Col);
            }, (Operators.ListSeparator returning) => {
                returning.AddValue(ParseExpression());
            });
        }

        private (List<string>, List<IOperator?>) ParseArgs() {
            return ParseLi<(List<string>, List<IOperator?>)>(() => {
                return (new List<string>(), new List<IOperator?>());
            }, ((List<string>, List<IOperator?>) returning) => {
                LexEntry key = Read();
                if(key.Type != TokenTypes.KEYWORD) {
                    Error("Expecting a function parameter!");
                }
                LexEntry next = Read();
                IOperator? exp = null;
                if(next.Val == "plant" || next.Val == "p") {
                    exp = ParseExpression();
                } else {
                    Stored = next;
                }
                returning.Item1.Add(key.Val);
                returning.Item2.Add(exp);
            });
        }

        private IOperator ParseAssignment(IOperator current, Func<IOperator, IOperator, IOperator> translate) {
            Print("parsing variable assignment");
            IOperator after = ParseCombiners();
            return new Operators.Assignment(current, translate(current, after), Row, Col);
        }

        private IOperator ParseExpression() {
            Print("begin expression");
            IOperator current = ParseCombiners();
            LexEntry next = Read();
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    switch(next.Val) {
                        case "plant":
                        case "p":
                            current = ParseAssignment(current, (IOperator current, IOperator after) => { return after; });
                            break;
                        case "+=":
                            current = ParseAssignment(current, (IOperator current, IOperator after) => { return new Operators.Add(stack, current, after, Row, Col); });
                            break;
                        case "-=":
                            current = ParseAssignment(current, (IOperator current, IOperator after) => { return new Operators.Subtract(stack, current, after, Row, Col); });
                            break;
                        case "*=":
                            current = ParseAssignment(current, (IOperator current, IOperator after) => { return new Operators.Multiply(stack, current, after, Row, Col); });
                            break;
                        case "/=":
                            current = ParseAssignment(current, (IOperator current, IOperator after) => { return new Operators.Divide(stack, current, after, Row, Col); });
                            break;
                        case "%=":
                            current = ParseAssignment(current, (IOperator current, IOperator after) => { return new Operators.Modulo(stack, current, after, Row, Col); });
                            break;
                        case "++":
                            current = new Operators.Assignment(current, new Operators.Add(stack, current, new Operators.Number(stack, 1, Row, Col), Row, Col), Row, Col);
                            break;
                        case "--":
                            current = new Operators.Assignment(current, new Operators.Subtract(stack, current, new Operators.Number(stack, 1, Row, Col), Row, Col), Row, Col);
                            break;
                        default:
                            Stored = next;
                            return false;
                    }
                    return true;
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
                        current = new Operators.And(stack, current, after, Row, Col);
                        return true;
                    }
                    if(next.Val == "||") {
                        Print("parsing or");
                        IOperator after = ParseComparators();
                        current = new Operators.Or(stack, current, after, Row, Col);
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
                        current = new Operators.EqualsEquals(stack, current, after, Row, Col);
                        return true;
                    }
                    if(next.Val == ">=") {
                        Print("parsing more than equals");
                        IOperator after = ParseTerms();
                        current = new Operators.MoreThanOrEquals(stack, current, after, Row, Col);
                        return true;
                    }
                    if(next.Val == "<=") {
                        Print("parsing less than equals");
                        IOperator after = ParseTerms();
                        current = new Operators.LessThanOrEquals(stack, current, after, Row, Col);
                        return true;
                    }
                    if(next.Val == ">") {
                        Print("parsing more than");
                        IOperator after = ParseTerms();
                        current = new Operators.MoreThan(stack, current, after, Row, Col);
                        return true;
                    }
                    if(next.Val == "<") {
                        Print("parsing less than");
                        IOperator after = ParseTerms();
                        current = new Operators.LessThan(stack, current, after, Row, Col);
                        return true;
                    }
                    if(next.Val == "!=") {
                        Print("parsing not equals");
                        IOperator after = ParseTerms();
                        current = new Operators.Invert(stack, new Operators.EqualsEquals(stack, current, after, Row, Col), Row, Col);
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
                        current = new Operators.Add(stack, current, after, Row, Col);
                        return true;
                    }
                    if(next.Val == "-") {
                        Print("parsing minus");
                        IOperator after = ParseFactors();
                        current = new Operators.Subtract(stack, current, after, Row, Col);
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
                        current = new Operators.Multiply(stack, current, after, Row, Col);
                        return true;
                    }
                    if(next.Val == "/") {
                        Print("parsing divide");
                        IOperator after = ParseNegatives();
                        current = new Operators.Divide(stack, current, after, Row, Col);
                        return true;
                    }
                    if(next.Val == "%") {
                        Print("parsing modulo");
                        IOperator after = ParseNegatives();
                        current = new Operators.Modulo(stack, current, after, Row, Col);
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
                    return new Operators.Multiply(stack, new Operators.Number(stack, -1.0, Row, Col), ParseNegatives(), Row, Col);
                }
                if(returned.Val == "!") {
                    return new Operators.Invert(stack, ParseNegatives(), Row, Col);
                }
            }
            Stored = returned;
            return ParseCalls();
        }
        
        private IOperator ParseCalls() {
            Print("begin calls");
            IOperator current = ParseLowest();
            LexEntry next = Read();
            Func<bool> checkCheck = (() => {
                if(next.Val == "(") {
                    Print("parsing function call");
                    IOperator args = ParseList();
                    RequireSymbol(")");
                    current = new Operators.FunctionCall(current, args, stack, Row, Col);
                    return true;
                }
                if(next.Val == "[") {
                    Print("parsing object accessor via array brackets");
                    IOperator exp = ParseExpression();
                    RequireSymbol("]");
                    current = new Operators.BracketGet(current, exp, stack, Row, Col);
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
                        LexEntry read = Read();
                        current = new Operators.Get(current, read.Val, stack, Row, Col);
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
                if(returned.Val == "dig" || returned.Val == "d") {
                    Print("parsing variable definition");
                    LexEntry next = Read();
                    List<string> modifiers = new List<string>();
                    while(next.Type == TokenTypes.OPERATOR && (next.Val == "public" || next.Val == "private" || next.Val == "protected" || next.Val == "static")) {
                        modifiers.Add(next.Val);
                        next = Read();
                    }
                    if(next.Type == TokenTypes.KEYWORD && next.Val != "this") { // can't declare a variable named "this"
                        LexEntry afterNext = Read();
                        if(afterNext.Type == TokenTypes.SYMBOL && afterNext.Val == "{") {
                            Print("parsing variable as property");
                            IOperator? give = null;
                            IOperator? _get = null;
                            for(int i = 0; i < 2; i++) {
                                LexEntry newType = Read();
                                if(newType.Type == TokenTypes.SYMBOL && newType.Val == "}") {
                                    Stored = newType;
                                    break;
                                }
                                if(newType.Type != TokenTypes.OPERATOR) {
                                    throw new RadishException($"Expecting plant or harvest function instead of {newType.Val}!");
                                }
                                RequireSymbol("{");
                                if(newType.Val == "plant" || newType.Val == "p") {
                                    give = new Operators.FunctionDefinition(stack, new List<string>() { "input" }, new List<IOperator?>() { null }, ParseScope(), Row, Col);

                                } else if(newType.Val == "harvest" || newType.Val == "h") {
                                    _get = new Operators.FunctionDefinition(stack, new List<string>(), new List<IOperator?>(), ParseScope(), Row, Col);
                                } else {
                                    throw new RadishException("Only plant and harvest functions are valid in this context!");
                                }
                                RequireSymbol("}");
                            }
                            RequireSymbol("}");
                            return new Operators.Property(stack, next.Val, give, _get, modifiers, Row, Col);
                        }
                        Stored = afterNext;
                        return new Operators.Declaration(stack, next.Val, modifiers, Row, Col);
                    }
                    throw new RadishException($"Expecting a variable name instead of {next.Val}!");
                }
                if(returned.Val == "tool" || returned.Val == "t") {
                    Print("parsing function definition");
                    RequireSymbol("(");
                    (List<string>, List<IOperator?>) args = ParseArgs();
                    RequireSymbol(")");
                    RequireSymbol("{");
                    Operators.ExpressionSeparator body = ParseScope();
                    RequireSymbol("}");
                    Operators.FunctionDefinition def = new Operators.FunctionDefinition(stack, args.Item1, args.Item2, body, Row, Col);
                    return def;
                }
                if(returned.Val == "null") {
                    Print("parsing NULL");
                    return new Operators.NullValue(Row, Col);
                }
                if(returned.Val == "all") {
                    Print("parsing ALL");
                    return new Operators.All(stack, Row, Col);
                }
                if(returned.Val == "throw") {
                    Print("parsing throw statement");
                    IOperator throwing = ParseExpression();
                    return new Operators.Throw(throwing, stack, Row, Col);
                }
                if(returned.Val == "import") {
                    Print("parsing import");
                    IOperator importing = ParseExpression();
                    return new Operators.Import(stack, importing, Row, Col);
                }
                if(returned.Val == "class") {
                    Print("parsing class definition");
                    string _base = "Object";
                    LexEntry next = Read();
                    if(next.Type == TokenTypes.SYMBOL && next.Val == ":") {
                        _base = Read().Val;
                    } else {
                        Stored = next;
                    }
                    RequireSymbol("{");
                    Print("parsing class body");
                    IOperator body = ParseScope();
                    Print("parsing closing braces");
                    RequireSymbol("}");
                    return new Operators.ClassDefinition(stack, body, _base, Row, Col);
                }
                if(returned.Val == "new") {
                    Print("parsing class instantiation");
                    LexEntry next = Read();
                    Print("parsing opening paren.");
                    RequireSymbol("(");
                    IOperator args = ParseList();
                    Print("parsing closing paren.");
                    RequireSymbol(")");
                    return new Operators.New(stack, next.Val, args, Row, Col);
                }
            } else if(returned.Type == TokenTypes.STRING) {
                Print("parsing string");
                return new Operators.String(stack, returned.Val, Row, Col);
            } else if(returned.Type == TokenTypes.NUMBER) {
                Print("parsing number");
                return new Operators.Number(stack, Double.Parse(returned.Val), Row, Col);
            } else if(returned.Type == TokenTypes.BOOLEAN) {
                Print("parsing boolean");
                return new Operators.Boolean(returned.Val == "yes", stack, Row, Col);
            } else if(returned.Type == TokenTypes.KEYWORD) {
                Print("parsing variable");
                return new Operators.Reference(stack, returned.Val, Row, Col);
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
                if(returned.Val == "{") {
                    Print("parsing object literal");
                    Operators.ExpressionSeparator body = ParseScope();
                    Print("parsing closing braces");
                    RequireSymbol("}");
                    return new Operators.ObjectDefinition(stack, body, Row, Col);
                }
            }
            throw Error($"Could not parse value: {returned.Val} !");
        }
    }
}