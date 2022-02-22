using System.Globalization;
namespace Tools {
    static class Operators {
        public static Dictionary<string, Operator> O;
        static Operators() {
            O = new Dictionary<string, Operator>() {
                { "+", new Operator(
                    new List<TokenTypes>() { TokenTypes.NUMBER, TokenTypes.NUMBER }, 
                    (List<LexEntry> args) => {
                        return new LexEntry(TokenTypes.NUMBER, $"{float.Parse(args[0].Val, CultureInfo.InvariantCulture) + float.Parse(args[1].Val, CultureInfo.InvariantCulture)}");
                    }, 
                    1
                ) }
            };
        }
    } 
}