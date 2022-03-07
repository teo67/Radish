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
            };
            Stored = null;
        }

        public static bool IsKeyword(string input) {
            return OpKeywords.Contains(input) || ScopeKeywords.Contains(input);
        }

        private static LexEntry Read(StreamReader reader) {
            if(Stored != null) {
                LexEntry saved = Stored;
                Stored = null;
                return saved;
            }
            return Lexer.Run(reader);
        }

        public static IOperator ParseScope(StreamReader reader) {
            //Console.WriteLine("begin scope");
            Operators.ExpressionSeparator returning = new Operators.ExpressionSeparator();
            LexEntry read = Read(reader);
            while(read.Type != TokenTypes.ENDOFFILE) {
                if(read.Type == TokenTypes.OPERATOR && ScopeKeywords.Contains(read.Val)) {
                    // do things with for loops and if statements and other block stuff (under construction)
                } else {
                    Stored = read;
                    returning.AddValue(ParseExpression(reader));
                    LexEntry next = Read(reader);
                    if(!(next.Type == TokenTypes.SYMBOL && next.Val == "r")) {
                        throw new Exception("Missing an endline character!");
                    }
                }
                read = Read(reader);
            }
            return returning;
        }

        private static IOperator ParseExpression(StreamReader reader) {
            //Console.WriteLine("begin expression");
            IOperator current = ParseCombiners(reader);
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

        private static IOperator ParseCombiners(StreamReader reader) {
            //Console.WriteLine("begin combiners");
            IOperator current = ParseComparators(reader);
            LexEntry next = Read(reader);
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "&&") {
                        IOperator after = ParseComparators(reader);
                        current = new Operators.And(current, after);
                        return true;
                    }
                    if(next.Val == "||") {
                        IOperator after = ParseComparators(reader);
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

        private static IOperator ParseComparators(StreamReader reader) {
            //Console.WriteLine("begin comparators");
            IOperator current = ParseTerms(reader);
            LexEntry next = Read(reader);
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "==") {
                        IOperator after = ParseTerms(reader);
                        current = new Operators.EqualsEquals(current, after);
                        return true;
                    }
                    if(next.Val == ">=") {
                        IOperator after = ParseTerms(reader);
                        current = new Operators.MoreThanOrEquals(current, after);
                        return true;
                    }
                    if(next.Val == "<=") {
                        IOperator after = ParseTerms(reader);
                        current = new Operators.LessThanOrEquals(current, after);
                        return true;
                    }
                    if(next.Val == ">") {
                        IOperator after = ParseTerms(reader);
                        current = new Operators.MoreThan(current, after);
                        return true;
                    }
                    if(next.Val == "<") {
                        IOperator after = ParseTerms(reader);
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

        private static IOperator ParseTerms(StreamReader reader) {
            //Console.WriteLine("begin terms");
            IOperator current = ParseFactors(reader);
            LexEntry next = Read(reader);
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "+") {
                        IOperator after = ParseFactors(reader);
                        current = new Operators.Add(current, after);
                        return true;
                    }
                    if(next.Val == "-") {
                        IOperator after = ParseFactors(reader);
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

        private static IOperator ParseFactors(StreamReader reader) {
            //Console.WriteLine("begin factors");
            IOperator current = ParseLowest(reader);
            LexEntry next = Read(reader);
            Func<bool> check = (() => {
                if(next.Type == TokenTypes.OPERATOR) {
                    if(next.Val == "*") {
                        IOperator after = ParseLowest(reader);
                        current = new Operators.Multiply(current, after);
                        return true;
                    }
                    if(next.Val == "/") {
                        IOperator after = ParseLowest(reader);
                        current = new Operators.Divide(current, after);
                        return true;
                    }
                    if(next.Val == "%") {
                        IOperator after = ParseLowest(reader);
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
        
        private static IOperator ParseLowest(StreamReader reader) {
            //Console.WriteLine("begin lowest");
            LexEntry returned = Read(reader);
            if(returned.Type == TokenTypes.OPERATOR) {
                if(returned.Val == "-") {
                    return new Operators.Multiply(new Operators.Number(-1.0), ParseLowest(reader));
                }
                if(returned.Val == "!") {
                    return new Operators.Invert(ParseLowest(reader));
                }
            } else if(returned.Type == TokenTypes.STRING) {
                return new Operators.String(returned.Val);
            } else if(returned.Type == TokenTypes.NUMBER) {
                return new Operators.Number(Double.Parse(returned.Val));
            } else if(returned.Type == TokenTypes.BOOLEAN) {
                return new Operators.Boolean(returned.Val == "yes");
            } else if(returned.Type == TokenTypes.KEYWORD) {
                // parse as variable (under construction)
                LexEntry next = Read(reader);
                if(next.Type == TokenTypes.SYMBOL && next.Val == "(") {
                    IOperator returning = (returned.Val == "output") ? new Operators.Output(ParseExpression(reader)) : throw new Exception("Method calls are under construction!");
                    LexEntry nextNext = Read(reader);
                    if(nextNext.Type == TokenTypes.SYMBOL && nextNext.Val == ")") {
                        return returning;
                    }
                    throw new Exception("Missing closing parentheses on method call!");
                } else {
                    Stored = next;
                    throw new Exception("Variable calls are under construction!");
                }
            } else if(returned.Type == TokenTypes.SYMBOL) {
                if(returned.Val == "(") {
                    IOperator returning = ParseExpression(reader);
                    LexEntry next = Read(reader);
                    if(next.Type == TokenTypes.SYMBOL && next.Val == ")") {
                        return returning;
                    }
                }
            }
            throw new Exception($"Could not parse value: {returned.Type}: {returned.Val} !");
        }
    }
}