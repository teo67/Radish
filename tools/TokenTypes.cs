namespace Tools {
    enum TokenTypes {
        SAME, // not used after lex stage, represents a continuation of the same token
        NONE, // not used after lex stage, represents token scheduled for deletion
        COMMENT, // also not used after lex stage
        STRING, 
        NUMBER,
        OPERATOR,
        BOOLEAN,
        END, // ends a given statement - these are always single character
        KEYWORD, 
        PROGRAM, // represents the entire program
        LINE, // represents a single line of the program
        ENDPROGRAM // specific end char, must have its own type
    }
}