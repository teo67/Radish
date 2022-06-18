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
        private bool IsStandard { get; }
        private Librarian Librarian { get; }
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
        public Operations(CountingReader reader, bool verbose, bool isStandard, Librarian librarian) {
            this.reader = reader;
            this.verbose = verbose;
            this.lexer = new Lexer(reader);
            this.Librarian = librarian;
            this.stack = new Stack(Librarian.FirstLayer);
            this.IsStandard = isStandard;
            Stored = null;
            PrevRow = -1;
            PrevCol = -1;
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
                Print(saved.Val);
                return saved;
            }
            PrevCol = reader.col;
            PrevRow = reader.row;
            LexEntry ran = lexer.Run();
            Print(ran.Val);
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

        private IOperator Parse(string name, Func<string, IOperator, IOperator?> run, Func<IOperator> previous) {
            Print($"Parsing {name}");
            IOperator current = previous();
            LexEntry? next = null;
            bool done = false;
            while(!done) {
                Print($"About to read for {name}");
                next = Read();
                done = true;
                if(next.Type == TokenTypes.OPERATOR) {
                    IOperator? result = run(next.Val, current);
                    if(result != null) {
                        current = result;
                        done = false;
                    }
                }
            }
            Stored = next;
            return current;
        }

        private IOperator ParseExpression() {
            List<Func<string, IOperator, Operators.SimpleOperator?>> others = new List<Func<string, IOperator, Operators.SimpleOperator?>>() {
                    IsCombiners, IsComparators, IsTerms, IsFactors
            };
            return Parse("Expression", (string val, IOperator current) => {
                if(val == "plant" || val == "p") {
                    return new Operators.Assignment(current, ParseCombiners(), Row, Col);
                } else if(val.EndsWith("=")) {
                    string edited = val.Substring(0, val.Length - 1);
                    Operators.SimpleOperator? returning = null;
                    for(int i = 0; i < others.Count && returning == null; i++) {
                        returning = others[i](edited, current);
                    }
                    if(returning != null) {
                        returning.IsAssigning = true; // mark as assignment
                    }
                    return returning;
                    }
                return null;
            }, ParseCombiners);
        }

        private Operators.SimpleOperator? IsCombiners(string val, IOperator current) {
            switch(val) {
                case "&&":
                    return new Operators.And(stack, current, ParseComparators(), Row, Col);
                case "||":
                    return new Operators.Or(stack, current, ParseComparators(), Row, Col);
                case "&":
                    return new Operators.BitwiseAnd(stack, current, ParseComparators(), Row, Col);
                case "|":
                    return new Operators.BitwiseOr(stack, current, ParseComparators(), Row, Col);
                case "^":
                    return new Operators.XOr(stack, current, ParseComparators(), Row, Col);
                default:
                    return null;
            }
        }
        private IOperator ParseCombiners() {
            return Parse("Combiners", IsCombiners, ParseComparators);
        }

        private Operators.SimpleOperator? IsComparators(string val, IOperator current) {
            switch(val) {
                case "==":
                    return new Operators.EqualsEquals(stack, current, ParseShifts(), Row, Col);
                case ">=":
                    return new Operators.MoreThanOrEquals(stack, current, ParseShifts(), Row, Col);
                case "<=":
                    return new Operators.LessThanOrEquals(stack, current, ParseShifts(), Row, Col);
                case ">":
                    return new Operators.MoreThan(stack, current, ParseShifts(), Row, Col);
                case "<":
                    return new Operators.LessThan(stack, current, ParseShifts(), Row, Col);
                case "!=":
                    return new Operators.NotEquals(stack, current, ParseShifts(), Row, Col);
                default:
                    return null;
            }
        }

        private IOperator ParseComparators() {
            return Parse("Comparators", IsComparators, ParseShifts);
        }

        private Operators.SimpleOperator? IsShifts(string val, IOperator current) {
            switch(val) {
                case ">>":
                    return new Operators.RightShift(stack, current, ParseTerms(), Row, Col);
                case "<<":
                    return new Operators.LeftShift(stack, current, ParseTerms(), Row, Col);
                default:
                    return null;
            }
        }

        private IOperator ParseShifts() {
            return Parse("Shifts", IsShifts, ParseTerms);
        }

        private Operators.SimpleOperator? IsTerms(string val, IOperator current) {
            switch(val) {
                case "+":
                    return new Operators.Add(stack, current, ParseFactors(), Row, Col);
                case "-":
                    return new Operators.Subtract(stack, current, ParseFactors(), Row, Col);
                default:
                    return null;
            }
        }

        private IOperator ParseTerms() {
            return Parse("Terms", IsTerms, ParseFactors);
        }

        private Operators.SimpleOperator? IsFactors(string val, IOperator current) {
            switch(val) {
                case "*":
                    return new Operators.Multiply(stack, current, ParseNegatives(), Row, Col);
                case "/":
                    return new Operators.Divide(stack, current, ParseNegatives(), Row, Col);
                case "%":
                    return new Operators.Modulo(stack, current, ParseNegatives(), Row, Col);
                default:
                    return null;
            }
        }

        private IOperator ParseFactors() {
            return Parse("Factors", IsFactors, ParseNegatives);
        }

        private IOperator ParseNegatives() {
            LexEntry returned = Read();
            if(returned.Type == TokenTypes.OPERATOR) {
                if(returned.Val == "-") {
                    return new Operators.Negative(stack, ParsePosts(), Row, Col);
                }
                if(returned.Val == "!") {
                    return new Operators.Invert(stack, ParsePosts(), Row, Col);
                }
                if(returned.Val == "~") {
                    return new Operators.Flip(stack, ParsePosts(), Row, Col);
                }
            }
            Stored = returned;
            return ParsePosts();
        }

        private IOperator ParsePosts() {
            Print("begin posts");
            IOperator before = ParseCalls();
            LexEntry? returned = null;
            bool done = false;
            while(!done) {
                Print("reading for posts");
                returned = Read();
                if(returned.Type == TokenTypes.OPERATOR) {
                    if(returned.Val == "++") {
                        before = new Operators.Increment(stack, before, Row, Col);
                    } else if(returned.Val == "--") {
                        before = new Operators.Decrement(stack, before, Row, Col);
                    } else {
                        done = true;
                    }
                } else {
                    done = true;
                }
            }
            Stored = returned;
            return before;
        }

        private IOperator ParseCalls(bool allowParens = true) {
            Print("begin calls");
            IOperator current = ParseLowest();
            LexEntry? next = null;
            bool done = false;
            while(!done) {
                done = true;
                Print("reading for calls");
                next = Read();
                if(next.Type == TokenTypes.SYMBOL) {
                    bool doneDone = false;
                    while(!doneDone) {
                        if(next.Val == "(" && allowParens) {
                            IOperator args = ParseList();
                            RequireSymbol(")");
                            current = new Operators.FunctionCall(current, args, stack, Row, Col);
                            next = Read();
                        } else if(next.Val == "[") {
                            IOperator exp = ParseExpression();
                            RequireSymbol("]");
                            current = new Operators.BracketGet(current, exp, stack, Row, Col);
                            next = Read();
                        } else {
                            doneDone = true;
                        }
                    }
                    if(next.Val == ".") {
                        current = new Operators.Get(current, Read().Val, stack, Row, Col);
                        done = false;
                    }
                }
            }
            Stored = next;
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
                    return new Operators.Import(stack, importing, Row, Col, reader.Filename, Librarian);
                }
                if(returned.Val == "class") {
                    Print("parsing class definition");
                    IOperator? _base = null;
                    LexEntry next = Read();
                    if(next.Type == TokenTypes.SYMBOL && next.Val == ":") {
                        _base = ParseExpression();
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
                    IOperator next = ParseCalls(false);
                    Print("parsing opening paren.");
                    RequireSymbol("(");
                    IOperator args = ParseList();
                    Print("parsing closing paren.");
                    RequireSymbol(")");
                    return new Operators.New(stack, next, args, Row, Col);
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
                if(IsStandard) {
                    return new Operators.StandardRef(stack, returned.Val, Row, Col, Librarian);
                }
                return new Operators.Reference(stack, returned.Val, Row, Col, Librarian);
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