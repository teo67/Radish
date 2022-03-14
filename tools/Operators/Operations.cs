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
    static class Operations {
        private static List<string> OpKeywords { get; }
        private static List<string> ScopeKeywords { get; }
        private static LexEntry? Stored { get; set; }
        static Operations() {
            OpKeywords = new List<string>() {
                // keywords that should be parsed as operators
            };
            ScopeKeywords = new List<string>() {
                // keywords that specifically trigger a new scope instance
                "if", "elseif", "else"
            };
            Stored = null;
        }

        public static bool IsKeyword(string input) {
            return OpKeywords.Contains(input) || ScopeKeywords.Contains(input);
        }

        private static void RequireSymbol(CountingReader reader, string input) {
            LexEntry next = Read(reader);
            if(!(next.Type == TokenTypes.SYMBOL && next.Val == input)) {
                throw reader.Error($"Missing expected symbol: {input}");
            }
        }

        private static LexEntry Read(CountingReader reader) {
            if(Stored != null) {
                LexEntry saved = Stored;
                Stored = null;
                return saved;
            }
            return Lexer.Run(reader);
        }

        public static Operators.ExpressionSeparator ParseScope(CountingReader reader, bool verbose) {
            if(verbose) {
                Console.WriteLine("begin scope");
            }
            Operators.ExpressionSeparator returning = new Operators.ExpressionSeparator();
            LexEntry read = Read(reader);
            while(read.Type != TokenTypes.ENDOFFILE && !(read.Type == TokenTypes.SYMBOL && read.Val == "}")) {
                if(read.Type == TokenTypes.OPERATOR && ScopeKeywords.Contains(read.Val)) {
                    // do things with for loops and if statements and other block stuff (under construction)
                    if(read.Val == "if") {
                        Stored = read;
                        returning.AddValue(ParseIfs(reader, verbose));
                    }
                } else {
                    if(verbose) {
                        Console.WriteLine("parsing expression");
                    }
                    Stored = read;
                    returning.AddValue(ParseExpression(reader, verbose));
                    if(verbose) {
                        Console.WriteLine("parsing endline (r)");
                    }
                    LexEntry next = Read(reader);
                    if(!(next.Type == TokenTypes.SYMBOL && next.Val == "r")) {
                        throw reader.Error("Missing an endline character!");
                    }
                }
                read = Read(reader);
            }
            Stored = read;
            return returning;
        }

        private static Operators.IfChain ParseIfs(CountingReader reader, bool verbose) {
            if(verbose) {
                Console.WriteLine("begin if chain");
            }
            Operators.IfChain returning = new Operators.IfChain();
            LexEntry IF = Read(reader);
            if(!(IF.Type == TokenTypes.OPERATOR && IF.Val == "if")) {
                throw reader.Error("Expecting if statement!");
            }
            returning.AddValue(ParseIf(reader, verbose));
            LexEntry read = Read(reader);
            while(read.Type == TokenTypes.OPERATOR && read.Val == "elseif") {
                returning.AddValue(ParseIf(reader, verbose));
                read = Read(reader);
            }
            if(read.Type == TokenTypes.OPERATOR && read.Val == "else") {
                RequireSymbol(reader, "{");
                IOperator scope = ParseScope(reader, verbose);
                RequireSymbol(reader, "}");
                returning.AddValue(new Operators.If(new Operators.Boolean(true), scope));
            } else {
                Stored = read;
            }
            return returning;
        }

        private static Operators.If ParseIf(CountingReader reader, bool verbose) {
            RequireSymbol(reader, "(");
            Operators.ListSeparator li = ParseList(reader, verbose);
            RequireSymbol(reader, ")");
            RequireSymbol(reader, "{");
            IOperator scope = ParseScope(reader, verbose);
            RequireSymbol(reader, "}");
            return new Operators.If(li.First(), scope);
        }

        private static Operators.ListSeparator ParseList(CountingReader reader, bool verbose) {
            if(verbose) {
                Console.WriteLine("begin list");
            }
            Operators.ListSeparator returning = new Operators.ListSeparator();
            LexEntry read = Read(reader);
            if(read.Type == TokenTypes.SYMBOL && (read.Val == "]" || read.Val == ")")) { // empty list
                Stored = read;
                return returning;
            }
            while(true) { 
                if(verbose) {
                    Console.WriteLine("parsing list element");
                }
                Stored = read;
                returning.AddValue(ParseExpression(reader, verbose));
                LexEntry next = Read(reader);

                if(!(next.Type == TokenTypes.SYMBOL && next.Val == ",")) {
                    Stored = next;
                    break;
                } else if(verbose) {
                    Console.WriteLine("found comma");
                }

                read = Read(reader);
            }
            return returning;
        }

        private static IOperator ParseExpression(CountingReader reader, bool verbose) {
            if(verbose) {
                Console.WriteLine("begin expression");
            }
            IOperator current = ParseCombiners(reader, verbose);
            LexEntry next = Read(reader);
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "=") {
                        // IOperator after = ParseCombiners(reader);
                        // current = new Operators.Assign(current, after);
                        // return true;
                        // assigning variables - under construction
                    }
                }
                Stored = next; // cancel viewing
                return false;
            });
            while(check()) {
                next = Read(reader);
            }
            return current;
        }

        private static IOperator ParseCombiners(CountingReader reader, bool verbose) {
            if(verbose) {
                Console.WriteLine("begin combiners");
            }
            IOperator current = ParseComparators(reader, verbose);
            LexEntry next = Read(reader);
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "&&") {
                        if(verbose) {
                            Console.WriteLine("parsing and");
                        }
                        IOperator after = ParseComparators(reader, verbose);
                        current = new Operators.And(current, after);
                        return true;
                    }
                    if(next.Val == "||") {
                        if(verbose) {
                            Console.WriteLine("parsing or");
                        }
                        IOperator after = ParseComparators(reader, verbose);
                        current = new Operators.Or(current, after);
                        return true;
                    }
                }
                Stored = next; // cancel viewing
                return false;
            });
            while(check()) {
                next = Read(reader);
            }
            return current;
        }

        private static IOperator ParseComparators(CountingReader reader, bool verbose) {
            if(verbose) {
                Console.WriteLine("begin comparators");
            }
            IOperator current = ParseTerms(reader, verbose);
            LexEntry next = Read(reader);
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "==") {
                        if(verbose) {
                            Console.WriteLine("parsing double equals");
                        }
                        IOperator after = ParseTerms(reader, verbose);
                        current = new Operators.EqualsEquals(current, after);
                        return true;
                    }
                    if(next.Val == ">=") {
                        if(verbose) {
                            Console.WriteLine("parsing more than equals");
                        }
                        IOperator after = ParseTerms(reader, verbose);
                        current = new Operators.MoreThanOrEquals(current, after);
                        return true;
                    }
                    if(next.Val == "<=") {
                        if(verbose) {
                            Console.WriteLine("parsing less than equals");
                        }
                        IOperator after = ParseTerms(reader, verbose);
                        current = new Operators.LessThanOrEquals(current, after);
                        return true;
                    }
                    if(next.Val == ">") {
                        if(verbose) {
                            Console.WriteLine("parsing more than");
                        }
                        IOperator after = ParseTerms(reader, verbose);
                        current = new Operators.MoreThan(current, after);
                        return true;
                    }
                    if(next.Val == "<") {
                        if(verbose) {
                            Console.WriteLine("parsing less than");
                        }
                        IOperator after = ParseTerms(reader, verbose);
                        current = new Operators.LessThan(current, after);
                        return true;
                    }
                }
                Stored = next; // cancel viewing
                return false;
            });
            while(check()) {
                next = Read(reader);
            }
            return current;
        }

        private static IOperator ParseTerms(CountingReader reader, bool verbose) {
            if(verbose) {
                Console.WriteLine("begin terms");
            }
            IOperator current = ParseFactors(reader, verbose);
            LexEntry next = Read(reader);
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "+") {
                        if(verbose) {
                            Console.WriteLine("parsing plus");
                        }
                        IOperator after = ParseFactors(reader, verbose);
                        current = new Operators.Add(current, after);
                        return true;
                    }
                    if(next.Val == "-") {
                        if(verbose) {
                            Console.WriteLine("parsing minus");
                        }
                        IOperator after = ParseFactors(reader, verbose);
                        current = new Operators.Subtract(current, after);
                        return true;
                    }
                }
                Stored = next; // cancel viewing
                return false;
            });
            while(check()) {
                next = Read(reader);
            }
            return current;
        }

        private static IOperator ParseFactors(CountingReader reader, bool verbose) {
            if(verbose) {
                Console.WriteLine("begin factors");
            }
            IOperator current = ParseLowest(reader, verbose);
            LexEntry next = Read(reader);
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "*") {
                        if(verbose) {
                            Console.WriteLine("parsing multiply");
                        }
                        IOperator after = ParseLowest(reader, verbose);
                        current = new Operators.Multiply(current, after);
                        return true;
                    }
                    if(next.Val == "/") {
                        if(verbose) {
                            Console.WriteLine("parsing divide");
                        }
                        IOperator after = ParseLowest(reader, verbose);
                        current = new Operators.Divide(current, after);
                        return true;
                    }
                    if(next.Val == "%") {
                        if(verbose) {
                            Console.WriteLine("parsing modulo");
                        }
                        IOperator after = ParseLowest(reader, verbose);
                        current = new Operators.Modulo(current, after);
                        return true;
                    }
                }
                Stored = next; // cancel viewing
                return false;
            });
            while(check()) {
                next = Read(reader);
            }
            return current;
        }
        
        private static IOperator ParseLowest(CountingReader reader, bool verbose) {
            if(verbose) {
                Console.WriteLine("begin lowest");
            }
            LexEntry returned = Read(reader);
            if(returned.Type == TokenTypes.OPERATOR) {
                if(returned.Val == "-") {
                    if(verbose) {
                        Console.WriteLine("parsing negative");
                    }
                    return new Operators.Multiply(new Operators.Number(-1.0), ParseLowest(reader, verbose));
                }
                if(returned.Val == "!") {
                    if(verbose) {
                        Console.WriteLine("parsing not");
                    }
                    return new Operators.Invert(ParseLowest(reader, verbose));
                }
            } else if(returned.Type == TokenTypes.STRING) {
                if(verbose) {
                    Console.WriteLine("parsing string");
                }
                return new Operators.String(returned.Val);
            } else if(returned.Type == TokenTypes.NUMBER) {
                if(verbose) {
                    Console.WriteLine("parsing number");
                }
                return new Operators.Number(Double.Parse(returned.Val));
            } else if(returned.Type == TokenTypes.BOOLEAN) {
                if(verbose) {
                    Console.WriteLine("parsing boolean");
                }
                return new Operators.Boolean(returned.Val == "yes");
            } else if(returned.Type == TokenTypes.KEYWORD) {
                // parse as variable (under construction)
                LexEntry next = Read(reader);
                if(next.Type == TokenTypes.SYMBOL && next.Val == "(") {
                    IOperator returning = (returned.Val == "output") ? new Operators.Output(ParseExpression(reader, verbose)) : throw reader.Error("Method calls are under construction!");
                    LexEntry nextNext = Read(reader);
                    if(nextNext.Type == TokenTypes.SYMBOL && nextNext.Val == ")") {
                        return returning;
                    }
                    throw reader.Error("Missing closing parentheses on method call!");
                } else {
                    Stored = next;
                    throw reader.Error("Variable calls are under construction!");
                }
            } else if(returned.Type == TokenTypes.SYMBOL) {
                if(returned.Val == "(") {
                    if(verbose) {
                        Console.WriteLine("parsing opening paren.");
                        Console.WriteLine("parsing expression");
                    }
                    IOperator returning = ParseExpression(reader, verbose);
                    LexEntry next = Read(reader);
                    if(verbose) {
                        Console.WriteLine("parsing closing paren.");
                    }
                    if(next.Type == TokenTypes.SYMBOL && next.Val == ")") {
                        return returning;
                    }
                }
            }
            throw reader.Error($"Could not parse value: {returned.Type}: {returned.Val} !");
        }
    }
}