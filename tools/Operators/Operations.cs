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
        private IReader reader { get; }
        private Lexer lexer { get; }
        public Stack stack { get; }
        private bool IsStandard { get; }
        private Librarian Librarian { get; }
        static Operations() {
            OpKeywords = new List<string>() {
                // keywords that should be parsed as operators
                "if", "elseif", "else", 
                "while", "for", "repeat",
                "dig", "d", "tool", "t", "plant", "p", "uproot",
                "harvest", "h", "cancel", "continue", "end", "fill",
                "new", "null", "class",
                "public", "private", "protected", "static", "type", "after", "and", "or", "not",
                "try", "catch", "throw", "import", "all", "PATH", "enum", "each", "of", "switch", "case", "default"
            };
        }
        public Operations(IReader reader, bool verbose, bool isStandard, Librarian librarian) {
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

        private bool OptionalSymbol(string input) {
            LexEntry next = Read();
            if(next.Type == TokenTypes.SYMBOL && next.Val == input) {
                return true;
            }
            Stored = next;
            return false;
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
            while(read.Type != TokenTypes.ENDOFFILE && !(read.Type == TokenTypes.SYMBOL && read.Val == "}") && !(read.Type == TokenTypes.OPERATOR && (read.Val == "case" || read.Val == "default"))) {
                if(read.Type == TokenTypes.OPERATOR && read.Val == "if") {
                    Stored = read;
                    returning.AddValue(ParseIfs());
                } else if(read.Type == TokenTypes.OPERATOR && read.Val == "while") {
                    bool res = OptionalSymbol("(");
                    IOperator exp = ParseExpression();
                    if(res) {
                        RequireSymbol(")");
                    }
                    RequireSymbol("{");
                    IOperator scope = ParseScope();
                    RequireSymbol("}");
                    returning.AddValue(new Operators.While(exp, scope, Row, Col));
                } else if(read.Type == TokenTypes.OPERATOR && read.Val == "for") {
                    bool res = OptionalSymbol("(");
                    Operators.ListSeparator list = ParseList();
                    if(res) {
                        RequireSymbol(")");
                    }
                    RequireSymbol("{");
                    IOperator body = ParseScope();
                    RequireSymbol("}");
                    returning.AddValue(new Operators.Loop(list, body, Row, Col));
                } else if(read.Type == TokenTypes.OPERATOR && read.Val == "repeat") {
                    bool res = OptionalSymbol("(");
                    IOperator next = ParseExpression();
                    if(res) {
                        RequireSymbol(")");
                    }
                    RequireSymbol("{");
                    IOperator body = ParseScope();
                    RequireSymbol("}");
                    returning.AddValue(new Operators.Repeat(next, body, Row, Col));
                } else if(read.Type == TokenTypes.OPERATOR && read.Val == "each") {
                    bool res = OptionalSymbol("(");
                    LexEntry newRead = Read();
                    LexEntry of = Read();
                    if(of.Type != TokenTypes.OPERATOR || of.Val != "of") {
                        throw Error($"Expecting 'of' instead of '{of.Val}'!");
                    }
                    IOperator li = ParseExpression();
                    if(res) {
                        RequireSymbol(")");
                    }
                    RequireSymbol("{");
                    IOperator body = ParseScope();
                    RequireSymbol("}");
                    returning.AddValue(new Operators.Each(newRead.Val, li, body, Row, Col));
                } else if(read.Type == TokenTypes.OPERATOR && read.Val == "switch") {
                    bool res = OptionalSymbol("(");
                    IOperator eval = ParseExpression();
                    if(res) {
                        RequireSymbol(")");
                    }
                    RequireSymbol("{");
                    List<IOperator> checks = new List<IOperator>();
                    List<IOperator> bodies = new List<IOperator>();
                    IOperator? def = null;
                    LexEntry next = Read();
                    while(next.Type != TokenTypes.ENDOFFILE && !(next.Val == "}" && next.Type == TokenTypes.SYMBOL)) {
                        if(next.Type == TokenTypes.OPERATOR && next.Val == "default") {
                            if(def == null) {
                                def = ParseScope();
                            } else {
                                throw Error("Switches may only contain one default scope!");
                            }
                        } else if(next.Type == TokenTypes.OPERATOR && next.Val == "case") {
                            checks.Add(ParseExpression());
                            bodies.Add(ParseScope());
                        } else {
                            throw Error("Switch statements must begin with a case or a default!");
                        }
                        next = Read();
                    }
                    returning.AddValue(new Operators.Switch(eval, checks, bodies, def, Row, Col));
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
                        returning.AddValue(new Operators.TryCatch(tryScope, catchScope, Row, Col));
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
                returning.AddValue(new Operators.If(new Operators.Boolean(true, Row, Col), scope, Row, Col));
            } else {
                Stored = read;
            }
            return returning;
        }

        private Operators.If ParseIf() {
            bool res = OptionalSymbol("(");
            IOperator li = ParseExpression();
            if(res) {
                RequireSymbol(")");
            }
            RequireSymbol("{");
            IOperator scope = ParseScope();
            RequireSymbol("}");
            return new Operators.If(li, scope, Row, Col);
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
                return new Operators.ListSeparator(Row, Col);
            }, (Operators.ListSeparator returning) => {
                returning.AddValue(ParseExpression());
            });
        }

        private List<IOperator> ParseCallArgs() {
            return ParseLi<List<IOperator>>(() => {
                return new List<IOperator>();
            }, (List<IOperator> returning) => {
                returning.Add(ParseExpression());
            });
        }

        private List<string> ParseEnum() {
            return ParseLi<List<string>>(() => {
                return new List<string>();
            }, (List<string> returning) => {
                returning.Add(Read().Val);
            });
        }

        private (List<string>, List<IOperator?>, bool) ParseArgs() {
            bool fill = false;
            (List<string>, List<IOperator?>) result = ParseLi<(List<string>, List<IOperator?>)>(() => {
                return (new List<string>(), new List<IOperator?>());
            }, ((List<string>, List<IOperator?>) returning) => {
                if(fill) {
                    throw new RadishException("If a function contains a 'fill' argument, it must be after all other arguments.", Row, Col);
                }
                LexEntry key = Read();
                if(key.Type == TokenTypes.OPERATOR && key.Val == "fill") {
                    fill = true;
                    key = Read();
                }
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
            return (result.Item1, result.Item2, fill);
        }

        private IOperator Parse(string name, Func<string, IOperator, Func<IOperator>, IOperator?> run, Func<IOperator> previous) {
            Print($"Parsing {name}");
            IOperator current = previous();
            LexEntry? next = null;
            bool done = false;
            while(!done) {
                Print($"About to read for {name}");
                next = Read();
                done = true;
                if(next.Type == TokenTypes.OPERATOR) {
                    IOperator? result = run(next.Val, current, previous);
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
            List<Func<string, IOperator, Func<IOperator>, Operators.SimpleOperator?>> others = new List<Func<string, IOperator, Func<IOperator>, Operators.SimpleOperator?>>() {
                    IsCombiners, IsComparators, IsShifts, IsTerms, IsFactors
            };
            return Parse("Expression", (string val, IOperator current, Func<IOperator> previous) => {
                if(val == "plant" || val == "p") {
                    return new Operators.Assignment(current, previous(), Row, Col);
                } else if(val.EndsWith("=")) {
                    string edited = val.Substring(0, val.Length - 1);
                    Operators.SimpleOperator? returning = null;
                    for(int i = 0; i < others.Count && returning == null; i++) {
                        returning = others[i](edited, current, previous);
                    }
                    if(returning != null) {
                        returning.IsAssigning = true; // mark as assignment
                    }
                    return returning;
                } else if(val == "after") {
                    return new Operators.After(current, previous(), Row, Col);
                }
                return null;
            }, ParseTernaries);
        }

        private IOperator ParseTernaries() {
            IOperator cond = ParseCombiners();
            LexEntry next = Read();
            if(next.Type == TokenTypes.OPERATOR && next.Val == "?") {
                IOperator first = ParseTernaries();
                LexEntry mid = Read();
                if(!(mid.Type == TokenTypes.SYMBOL && mid.Val == ",")) {
                    throw Error("Expecting a comma to complete the ternary!");
                } 
                IOperator second = ParseTernaries();
                return new Operators.Ternary(cond, first, second, Row, Col);
            }
            Stored = next;
            return cond;
        }

        private Operators.SimpleOperator? IsCombiners(string val, IOperator current, Func<IOperator> previous) {
            switch(val) {
                case "&&":
                case "and":
                    return new Operators.And(current, previous(), Row, Col);
                case "||":
                case "or":
                    return new Operators.Or(current, previous(), Row, Col);
                case "&":
                    return new Operators.BitwiseAnd(current, previous(), Row, Col);
                case "|":
                    return new Operators.BitwiseOr(current, previous(), Row, Col);
                case "^":
                    return new Operators.XOr(current, previous(), Row, Col);
                default:
                    return null;
            }
        }
        private IOperator ParseCombiners() {
            return Parse("Combiners", IsCombiners, ParseComparators);
        }

        private Operators.SimpleOperator? IsComparators(string val, IOperator current, Func<IOperator> previous) {
            switch(val) {
                case "=":
                    return new Operators.EqualsEquals(current, previous(), Row, Col);
                case ">=":
                    return new Operators.MoreThanOrEquals(current, previous(), Row, Col);
                case "<=":
                    return new Operators.LessThanOrEquals(current, previous(), Row, Col);
                case ">":
                    return new Operators.MoreThan(current, previous(), Row, Col);
                case "<":
                    return new Operators.LessThan(current, previous(), Row, Col);
                case "!=":
                    return new Operators.NotEquals(current, previous(), Row, Col);
                default:
                    return null;
            }
        }

        private IOperator ParseComparators() {
            return Parse("Comparators", IsComparators, ParseShifts);
        }

        private Operators.SimpleOperator? IsShifts(string val, IOperator current, Func<IOperator> previous) {
            switch(val) {
                case ">>":
                    return new Operators.RightShift(current, previous(), Row, Col);
                case "<<":
                    return new Operators.LeftShift(current, previous(), Row, Col);
                default:
                    return null;
            }
        }

        private IOperator ParseShifts() {
            return Parse("Shifts", IsShifts, ParseTerms);
        }

        private Operators.SimpleOperator? IsTerms(string val, IOperator current, Func<IOperator> previous) {
            switch(val) {
                case "+":
                    return new Operators.Add(current, previous(), Row, Col);
                case "-":
                    return new Operators.Subtract(current, previous(), Row, Col);
                default:
                    return null;
            }
        }

        private IOperator ParseTerms() {
            return Parse("Terms", IsTerms, ParseFactors);
        }

        private Operators.SimpleOperator? IsFactors(string val, IOperator current, Func<IOperator> previous) {
            switch(val) {
                case "*":
                    return new Operators.Multiply(current, previous(), Row, Col);
                case "/":
                    return new Operators.Divide(current, previous(), Row, Col);
                case "%":
                    return new Operators.Modulo(current, previous(), Row, Col);
                default:
                    return null;
            }
        }

        private IOperator ParseFactors() {
            return Parse("Factors", IsFactors, ParseExponents);
        }

        private Operators.SimpleOperator? IsExponent(string val, IOperator current, Func<IOperator> previous) {
            if(val == "**") {
                return new Operators.Exponent(current, previous(), Row, Col);
            }
            return null;
        }

        private IOperator ParseExponents() {
            return Parse("Exponents", IsExponent, ParseNegatives);
        }

        private IOperator ParseNegatives() {
            LexEntry returned = Read();
            if(returned.Type == TokenTypes.OPERATOR) {
                if(returned.Val == "-") {
                    return new Operators.Negative(ParseNegatives(), Row, Col);
                }
                if(returned.Val == "!" || returned.Val == "not") {
                    return new Operators.Invert(ParseNegatives(), Row, Col);
                }
                if(returned.Val == "~") {
                    return new Operators.Flip(ParseNegatives(), Row, Col);
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
                        before = new Operators.Increment(before, Row, Col);
                    } else if(returned.Val == "--") {
                        before = new Operators.Decrement(before, Row, Col);
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
                Print("reading for calls");
                next = Read();
                if(next.Type == TokenTypes.SYMBOL) {
                    bool doneDone = false;
                    while(!doneDone) {
                        if(next.Type == TokenTypes.SYMBOL) {
                            if(next.Val == "(" && allowParens) {
                                List<IOperator> args = ParseCallArgs();
                                RequireSymbol(")");
                                current = new Operators.FunctionCall(current, args, Row, Col);
                                next = Read();
                            } else if(next.Val == "[") {
                                IOperator exp = ParseExpression();
                                RequireSymbol("]");
                                current = new Operators.BracketGet(current, exp, Row, Col);
                                next = Read();
                            } else {
                                doneDone = true;
                            }
                        } else {
                            doneDone = true;
                        }
                    }
                    if(next.Val == ".") {
                        current = new Operators.Get(current, Read().Val, Row, Col);
                    } else if(next.Val == ":") {
                        current = new Operators.Delete(current, Read().Val, Row, Col);
                    } else {
                        done = true;
                    }
                } else {
                    done = true;
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
                                    give = new Operators.FunctionDefinition(new List<string>() { "input" }, new List<IOperator?>() { null }, false, ParseScope(), Row, Col, reader.Filename);

                                } else if(newType.Val == "harvest" || newType.Val == "h") {
                                    _get = new Operators.FunctionDefinition(new List<string>(), new List<IOperator?>(), false, ParseScope(), Row, Col, reader.Filename);
                                } else {
                                    throw new RadishException("Only plant and harvest functions are valid in this context!");
                                }
                                RequireSymbol("}");
                            }
                            RequireSymbol("}");
                            return new Operators.Property(next.Val, give, _get, modifiers, Row, Col);
                        }
                        Stored = afterNext;
                        return new Operators.Declaration(next.Val, modifiers, Row, Col);
                    }
                    throw new RadishException($"Expecting a variable name instead of {next.Val}!");
                }
                if(returned.Val == "uproot") {
                    Print("parsing uproot call");
                    LexEntry next = Read();
                    return new Operators.Uproot(next.Val, Row, Col);
                }
                if(returned.Val == "tool" || returned.Val == "t") {
                    Print("parsing function definition");
                    RequireSymbol("(");
                    (List<string>, List<IOperator?>, bool) args = ParseArgs();
                    RequireSymbol(")");
                    RequireSymbol("{");
                    Operators.ExpressionSeparator body = ParseScope();
                    RequireSymbol("}");
                    Operators.FunctionDefinition def = new Operators.FunctionDefinition(args.Item1, args.Item2, args.Item3, body, Row, Col, reader.Filename);
                    return def;
                }
                if(returned.Val == "null") {
                    Print("parsing NULL");
                    return new Operators.NullValue(Row, Col);
                }
                if(returned.Val == "all") {
                    Print("parsing ALL");
                    return new Operators.All(Row, Col);
                }
                if(returned.Val == "PATH") {
                    Print("parsing PATH");
                    return new Operators.String(new List<string>() { reader.Filename }, new List<IOperator>(), Row, Col);
                }
                if(returned.Val == "throw") {
                    Print("parsing throw statement");
                    IOperator throwing = ParseExpression();
                    return new Operators.Throw(throwing, Row, Col);
                }
                if(returned.Val == "type") {
                    Print("parsing type statement");
                    IOperator typing = ParseExpression();
                    return new Operators.Type(typing, Row, Col);
                }
                if(returned.Val == "import") {
                    Print("parsing import");
                    IOperator importing = ParseExpression();
                    return new Operators.Import(importing, Row, Col, reader.Filename, Librarian, IsStandard);
                }
                if(returned.Val == "class") {
                    Print("parsing class definition");
                    IOperator? _base = null;
                    LexEntry next = Read();
                    if(next.Type == TokenTypes.OPERATOR && next.Val == "after") {
                        _base = ParseExpression();
                    } else {
                        Stored = next;
                    }
                    RequireSymbol("{");
                    Print("parsing class body");
                    IOperator body = ParseScope();
                    Print("parsing closing braces");
                    RequireSymbol("}");
                    return new Operators.ClassDefinition(body, _base, Row, Col, reader.Filename);
                }
                if(returned.Val == "enum") {
                    Print("parsing enum");
                    RequireSymbol("{");
                    List<string> list = ParseEnum();
                    RequireSymbol("}");
                    return new Operators.Enum(list, Row, Col);
                }
                if(returned.Val == "new") {
                    Print("parsing class instantiation");
                    IOperator next = ParseCalls(false);
                    Print("parsing opening paren.");
                    RequireSymbol("(");
                    List<IOperator> args = ParseCallArgs();
                    Print("parsing closing paren.");
                    RequireSymbol(")");
                    return new Operators.New(next, args, Row, Col);
                }
            } else if(returned.Type == TokenTypes.STRING) {
                if(returned.Val[0] == '\'') {
                    throw new RadishException("Strings may not be declared using single quotes!", Row, Col);
                }
                List<string> stringParts = new List<string>() { returned.Val.Substring(1, returned.Val.Length - 2) };
                List<IOperator> interpolations = new List<IOperator>();
                while(returned.Val[returned.Val.Length - 1] == '\'') {
                    interpolations.Add(ParseExpression());
                    returned = Read();
                    if(returned.Type != TokenTypes.STRING) {
                        throw new RadishException("Strings may not be ended using single quotes!", Row, Col);
                    }
                    if(returned.Val[0] != '\'') {
                        throw new RadishException("String interpolations must begin and end with single quotes!", Row, Col);
                    }
                    stringParts.Add(returned.Val.Substring(1, returned.Val.Length - 2));
                }
                Print("parsing string");
                return new Operators.String(stringParts, interpolations, Row, Col);
            } else if(returned.Type == TokenTypes.NUMBER) {
                Print("parsing number");
                return new Operators.Number(Double.Parse(returned.Val), Row, Col);
            } else if(returned.Type == TokenTypes.BOOLEAN) {
                Print("parsing boolean");
                return new Operators.Boolean(returned.Val == "yes", Row, Col);
            } else if(returned.Type == TokenTypes.KEYWORD) {
                Print("parsing variable");
                if(IsStandard) {
                    return new Operators.StandardRef(returned.Val, Row, Col, Librarian);
                }
                return new Operators.Reference(returned.Val, Row, Col, Librarian);
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
                    return new Operators.ObjectDefinition(body, null, Row, Col);
                }
            }
            throw Error($"Could not parse value: {returned.Val} !");
        }
    }
}