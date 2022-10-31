namespace Tools {
    class MiniStackNode {
        public string? Val { get; set; }
        public MiniStackNode? Next { get; set; }
        public MiniStackNode(MiniStackNode? previous = null, string? value = null) {
            this.Val = value;
            this.Next = previous;
        }
    }
    class Minifier : Operations {
        public string Output { get; private set; }
        private LexEntry Last { get; set; }
        private Dictionary<string, string> VariableKey { get; }
        private List<int> Current { get; }
        private MinifyOptions Options { get; }
        private static string ValidChars { get; }
        private static string Nums { get; }
        private static string[] NoNoWords { get; }
        private LexEntry? realStored;
        private MiniStackNode? LastVariable { get; set; }
        private int LastLength { get; set; }
        static Minifier() {
            ValidChars = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Nums = "0123456789";
            NoNoWords = new string[4] { "this", "prototype", "super", "constructor" };
        }
        public Minifier(IReader reader, Librarian librarian, MinifyOptions options, bool verbose) : base(reader, verbose, false, librarian) {
            Output = "";
            Options = options;
            Last = new LexEntry(TokenTypes.NONE, "");
            VariableKey = new Dictionary<string, string>();
            Current = new List<int>();
            LastVariable = null;
            realStored = null;
            LastLength = 0;
        }
        protected override LexEntry? Stored {
            get {
                return realStored;
            }
            set {
                if(!String.IsNullOrEmpty(Output)) {
                    Output = Output.Substring(0, Output.Length - LastLength);
                }
                LastLength = 0;
                realStored = value;
            }
        }
        private bool IsWord(LexEntry input) {
            return (input.Type == TokenTypes.KEYWORD || input.Type == TokenTypes.BOOLEAN || (input.Type == TokenTypes.OPERATOR && OpKeywords.Contains(input.Val)));
        }
        private void UpdateCurrent() {
            int currentIndex = Current.Count - 1;
            while(currentIndex >= 0) {
                if(currentIndex == 0) {
                    if(Current[currentIndex] != ValidChars.Length - 1) {
                        break;
                    }
                } else if(Current[currentIndex] != ValidChars.Length + Nums.Length - 1) {
                    break;
                }
                currentIndex--;
            }
            for(int i = currentIndex + 1; i < Current.Count; i++) {
                Current[i] = 0;
            }
            if(currentIndex == -1) {
                Current.Add(0);
            } else {
                Current[currentIndex]++;
            }
        }
        private string GetCurrent() {
            string returning = "";
            for(int i = 0; i < Current.Count; i++) {
                returning += (Current[i] < ValidChars.Length) ? ValidChars[Current[i]] : Nums[Current[i] - ValidChars.Length];
            }
            return returning;
        }
        protected override LexEntry Read() {
            LexEntry ran;
            if(Stored != null) {
                ran = Stored;
                Stored = null;
            } else {
                PrevCol = reader.col;
                PrevRow = reader.row;
                ran = lexer.Run();
                if(
                    (IsWord(Last) && IsWord(ran)) || (IsWord(Last) && ran.Type == TokenTypes.NUMBER) || (Last.Type == TokenTypes.OPERATOR && ran.Type == TokenTypes.OPERATOR)
                ) {
                    Output += " ";
                }
            }
            string adding = "";
            if(ran.Type == TokenTypes.KEYWORD && LastVariable != null && LastVariable.Val == null) {
                LastVariable.Val = ran.Val;
            }
            if(Options.VariableNames && !NoNoWords.Contains(ran.Val) && ran.Type == TokenTypes.KEYWORD && !Librarian.Standard.Contains(ran.Val) && (LastVariable == null || LastVariable.Val == null || !Librarian.Standard.Contains(LastVariable.Val))) {
                string? t = null;
                bool got = VariableKey.TryGetValue(ran.Val, out t);
                if(t != null && got) {
                    adding += t;
                } else {
                    string? gotten = null;
                    do {
                        UpdateCurrent();
                        gotten = GetCurrent();
                    } while(gotten == null || Operations.OpKeywords.Contains(gotten) || gotten == "this");
                    VariableKey.Add(ran.Val, gotten);
                    adding += gotten;
                }
            } else {
                adding += ran.Val;
            }
            LastLength = adding.Length;
            Output += adding;
            Last = ran;
            return ran;
        }
        protected override void Handle(string input) {
            if(!String.IsNullOrEmpty(Output) && Options.ShortenKeywords && (input == "harvest" || input == "plant" || input == "dig" || input == "tool") || (Options.FunctionOptimizations && input == "FUN")) {
                Output = Output.Substring(0, Output.Length - input.Length + 1);
                if(input == "FUN") {
                    Last = new LexEntry(TokenTypes.OPERATOR, "t");
                }
            } else if(input == "RESET") {
                LastVariable = new MiniStackNode(LastVariable);
            } else if(input == "PROXY" && LastVariable != null && LastVariable.Val == null) {
                LastVariable.Val = "";
            } else if(input == "POP" && LastVariable != null) {
                LastVariable = LastVariable.Next;
            }
        }
        private Stack CreateEmptyStack() {
            return new Stack(new StackNode(new Dictionary<string, Values.Variable>()));
        }
        protected override IOperator ParseImport() {
            string savedOutput = (!String.IsNullOrEmpty(Output) && Output.EndsWith("import")) ? Output.Substring(0, Output.Length - 6) : Output;
            IOperator exp = ParseExpression();
            if(Options.IncludeImports) {
                try {
                    string res = exp._Run(CreateEmptyStack()).String;
                    IOperator minifyImport = new Operators.MinifyImport(exp, reader.Filename, Librarian, Options);
                    string newOutput = minifyImport._Run(CreateEmptyStack()).String;
                    Output = savedOutput + $"t{{{newOutput}}}()";
                } catch {
                    Console.WriteLine($"WARNING: import on line {Row} is being left un-minified, as the target path is unclear (this may be caused by a circular dependency).");
                }
            }
            return exp;
        }
    }
}