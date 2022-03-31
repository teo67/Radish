namespace Tools { // adds basic prototypes to call stack
    class Prototypes {
        public Values.Variable Number { get; }
        public Values.Variable output { get; }
        public Values.Variable String { get; }
        public Values.Variable Array { get; }
        public Prototypes(Stack stack) {
            List<Values.Variable> top = stack.Push().Val;
            Number = new Values.Variable("Number", new Values.ObjectLiteral(new List<Values.Variable>() {
                new Values.Variable("getValue", new Values.FunctionLiteral(
                    stack, new List<string>(), new Operators.ExpressionSeparator(
                        new List<IOperator>() {
                            new Operators.ReturnType("out", new Operators.Reference(stack, "this"))
                        }
                    )
                ))
            }));
            top.Add(Number);
            output = new Values.Variable("output", new Values.FunctionLiteral(stack, 
                        new List<string>() { "input" }, new Operators.ExpressionSeparator(
                            new List<IOperator>() {
                                new Operators.ReturnType("out", new Operators.Output(
                                        new Operators.Reference(stack, "input")
                                )
                            )
                        }
                    )
                )
            );
            top.Add(output);
            String = new Values.Variable("String", new Values.ObjectLiteral(new List<Values.Variable>() {
                new Values.Variable("getValue", new Values.FunctionLiteral(
                    stack, new List<string>(), new Operators.ExpressionSeparator(
                        new List<IOperator>() {
                            new Operators.ReturnType("out", new Operators.Reference(stack, "this"))
                        }
                    )
                )),
                new Values.Variable("length", new Values.FunctionLiteral(
                    stack, new List<string>(), new Operators.ExpressionSeparator(
                        new List<IOperator>() {
                            new Operators.ReturnType("out", new Operators.LengthOf(new Operators.Reference(stack, "this"), Number))
                        }
                    )
                ))
            }));
            String.Object.Add(new Values.Variable("lowercase", new Values.FunctionLiteral( // add this after bc it refers to string
                    stack, new List<string>(), new Operators.ExpressionSeparator(
                        new List<IOperator>() {
                            new Operators.ReturnType("out", new Operators.Lowercase(new Operators.Reference(stack, "this"), String))
                        }
                    )
            )));
            top.Add(String);
            Array = new Values.Variable("Array", new Values.ObjectLiteral(new List<Values.Variable>() {
                new Values.Variable("getValue", new Values.FunctionLiteral(
                    stack, new List<string>(), new Operators.ExpressionSeparator(
                        new List<IOperator>() {
                            new Operators.ReturnType("out", new Operators.Reference(stack, "this"))
                        }
                    )
                )),
                new Values.Variable("push", new Values.FunctionLiteral(
                    stack, new List<string>() { "input" }, new Operators.ExpressionSeparator(
                        new List<IOperator>() {
                            new Operators.ReturnType("out", new Operators.Push(new Operators.Reference(stack, "this"), new Operators.Reference(stack, "input")))
                        }
                    )
                ))
            }));
            top.Add(Array);
        }
    }
}