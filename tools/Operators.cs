using System.Globalization;
namespace Tools {
    static class Operators {
        public static Dictionary<string, ProtoOperator> O;
        public static ProtoOperator Function;
        static Operators() {
            O = new Dictionary<string, ProtoOperator>() {
                { "Program", new ProtoOperator(
                    (AbstractSyntaxNode viewing) => {
                        return viewing.Val.Type == TokenTypes.ENDPROGRAM;
                    }, -1 // infinity
                )},
                { "Line", new ProtoOperator(
                    (AbstractSyntaxNode viewing) => {
                        return viewing.Val.Type == TokenTypes.END && viewing.Val.Val == "r";
                    }, 2
                )},
                { "+", new ProtoOperator(
                    (AbstractSyntaxNode viewing) => {
                        return true;
                    }, 2
                )}
            };
            Function = new ProtoOperator(
                (AbstractSyntaxNode viewing) => {
                    return viewing.Val.Type == TokenTypes.OPERATOR && viewing.Val.Val == "(";
                }, 1
            );
        }
    } 
}