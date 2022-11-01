/*
 the minify algorithm is still in progress, but it works pretty well so far!
 minified files also minify all standard library items that they use and tell the interpreter to disable stdlib usage,
 which speeds up read time
*/
namespace Tools {
    class Minifier : Operations {
        public string Output { get; private set; }
        private LexEntry Last { get; set; }
        private Dictionary<string, string> VariableKey { get; }
        private List<int> Current { get; }
        private MinifyOptions Options { get; }
        private static string ValidChars { get; }
        private static string Nums { get; }
        private static string[] NoNoWords { get; }
        private List<string> StandardsUsed { get; }
        static Minifier() {
            ValidChars = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Nums = "0123456789";
            NoNoWords = new string[] { "this", "prototype", "super", "constructor", "Array", "Boolean", "Function", "Number", "Object", "Poly", "String" };
        }
        public Minifier(IReader reader, Librarian librarian, MinifyOptions options, bool verbose, bool isStandard = false, Dictionary<string, string>? linkedKey = null, List<int>? linkedCurrent = null, List<string>? linkedStdi = null) : base(reader, verbose, isStandard, librarian) {
            Output = "";
            Options = options;
            Last = new LexEntry(TokenTypes.NONE, "", "");
            VariableKey = linkedKey == null ? new Dictionary<string, string>() : linkedKey;
            Current = linkedCurrent == null ? new List<int>() : linkedCurrent;
            StandardsUsed = linkedStdi == null ? new List<string>() : linkedStdi;
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
        private void AddEntry(LexEntry ran) {
            if(
                (IsWord(Last) && IsWord(ran)) || (IsWord(Last) && ran.Type == TokenTypes.NUMBER) || (Last.Type == TokenTypes.OPERATOR && ran.Type == TokenTypes.OPERATOR)
            ) {
                Output += " ";
            }
            string adding = "";
            if(Options.VariableNames && !NoNoWords.Contains(ran.Val) && ran.Type == TokenTypes.KEYWORD && !(IsStandard && Librarian.StandardSpecials.ContainsKey(ran.Val))) {
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
                adding += (ran.Raw == null) ? ran.Val : ran.Raw;
            }
            Output += adding;
        }
        protected override LexEntry Read() {
            if(Stored != null) {
                LexEntry prev = Stored;
                Stored = null;
                return prev;
            }
            PrevCol = reader.col;
            PrevRow = reader.row;
            LexEntry raw = lexer.Run();
            AddEntry(raw);
            Last = raw;
            return raw;
        }
        protected override void Handle(string input) {
            if(!String.IsNullOrEmpty(Output) && Options.ShortenKeywords && (input == "harvest" || input == "plant" || input == "dig" || input == "tool") || (Options.FunctionOptimizations && input == "FUN")) {
                Output = Output.Substring(0, Output.Length - input.Length + 1);
                if(input == "FUN") {
                    Last = new LexEntry(TokenTypes.OPERATOR, "t", "t");
                }
            } else if(input == "CHECK" && !IsStandard) {
                if(Librarian.Standard.Contains(Last.Val) && !StandardsUsed.Contains(Last.Val)) {
                    if(!Options.SkipStdPrompts) {
                        Console.Write($"[y/n] Does your program make use of the {Last.Val} section of the standard library (default: yes)? ");
                        string? response = Console.ReadLine();
                        if(response == "n" || response == "no") {
                            return;
                        }
                    }
                    StandardsUsed.Add(Last.Val);
                }
            }
        }
        private Stack CreateEmptyStack() {
            return new Stack(new StackNode(new Dictionary<string, Values.Variable>()));
        }
        protected override IOperator ParseImport() {
            string savedOutput = Output.Substring(0, Output.Length - 6);
            LexEntry? savedLast = Last;
            IOperator exp = ParseExpression();
            savedLast = (Last == savedLast) ? null : Last;
            if(Options.IncludeImports) {
                try {
                    string res = exp._Run(CreateEmptyStack()).String;
                    IOperator minifyImport = new Operators.MinifyImport(exp, reader.Filename, Librarian, Options, VariableKey, Current, StandardsUsed, IsStandard);
                    string newOutput = minifyImport._Run(CreateEmptyStack()).String;
                    Output = savedOutput + $"t{{{newOutput}}}()";
                    Last = new LexEntry(TokenTypes.SYMBOL, ")", ")");
                    if(savedLast != null) {
                        AddEntry(savedLast);
                        Last = savedLast;
                    }
                } catch {
                    Console.WriteLine($"WARNING: import on line {Row} is being left un-minified, as the target path is unclear (this may be caused by a circular dependency).");
                }
            }
            return exp;
        }
        public void HandleStandardLibraryUsage() {
            if(!IsStandard) {
                string atTop = "";
                StandardsUsed.Insert(0, "PROTOTYPES");
                for(int i = 0; i < StandardsUsed.Count; i++) {
                    string? val;
                    bool gotten = VariableKey.TryGetValue(StandardsUsed[i], out val);
                    if(val == null && StandardsUsed[i] != "PROTOTYPES") {
                        continue;
                    }
                    string path = Librarian.GetPath(StandardsUsed[i], -1, -1);
                    IOperator importer = new Operators.StandardMinifyImport(path, Librarian, Options, VariableKey, Current, StandardsUsed);
                    string newOutput = importer._Run(CreateEmptyStack()).String;
                    if(i > 1) {
                        atTop += " ";
                    }
                    atTop += $"t{{{newOutput}}}()";
                    if(StandardsUsed[i] != "PROTOTYPES") {
                        atTop += $"=>{val}";
                    }
                }
                Output = $"`{atTop}`if yes{{{Output}}}";
            }
        }
    }
}