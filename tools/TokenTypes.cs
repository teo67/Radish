namespace Tools {
    enum TokenTypes {
        SAME, // not used after lex stage, represents a continuation of the same token
        NONE, // not used after lex stage, represents token scheduled for deletion
        COMMENT, // also not used after lex stage
        STRING, 
        NUMBER,
        OPERATOR,
        BOOLEAN,
        ENDLINE
    }
}