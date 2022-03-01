 /* priority list    
        0: scope - entire script, if statement, for loop etc
        1: expression - an entire line of code (does not include endline char, that gets checked for in layer 0 if necessary)
            examples: literally just the = operator
        2: boolean operators: &&, ||
        3: more boolean operators: ==
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
namespace Tools.Operators {
    static class Operators {
        public static List<string> opKeywords;
        static Operators() {
            opKeywords = new List<string>() {
                // keywords that should be parsed as operators
            };
        }

        public static void Test() {
            Console.WriteLine(new NumberLiteral(3).Run<double>());
        }

        // static IOperator ParseScope(StreamReader reader, Func<LexEntry, Boolean> isDone) {
        //     LexEntry read = Lexer.Run(reader);
        //     while(!isDone(read)) {
        //         if(false) {
        //         // scope blocks like if, for, while
        //         } else {
        //             List<IOperator> opGroup = new List<IOperator>() {
        //                 new NumberLiteral(3),
        //                 new NumberLiteral(3)
        //             };
        //             foreach(IOperator op in opGroup) {
        //                 Console.WriteLine(op.RunDouble());
        //             }
        //         }
        //     }
            
        // }
        
    }
}