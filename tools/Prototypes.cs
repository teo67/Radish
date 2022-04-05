namespace Tools { // adds basic prototypes to call stack
    class Prototypes {
        public Prototypes(Stack stack) {
            List<Values.Variable> top = stack.Push().Val;
            IValue Object = new Values.ObjectLiteral(new List<Values.Variable>(), null);
            top.Add(new Values.Variable("Object", Object));
            IValue Number = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            top.Add(new Values.Variable("Number", Number));
            IValue String = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            top.Add(new Values.Variable("String", String));
            IValue Array = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            top.Add(new Values.Variable("Array", Array));
            IValue Boolean = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            top.Add(new Values.Variable("Boolean", Boolean));
            IValue Function = new Values.ObjectLiteral(new List<Values.Variable>(), Object);
            top.Add(new Values.Variable("Function", Function));

            Object.Object.Add(new Values.Variable("getValue", new Values.FunctionLiteral(
                    stack, new List<string>(), new Operators.ExpressionSeparator(
                        new List<IOperator>() {
                            new Operators.ReturnType("out", new Operators.Reference(stack, "this"))
                        }
                    )
            , Function)));

            String.Object.Add(new Values.Variable("length", new Values.FunctionLiteral(
                    stack, new List<string>(), new Operators.ExpressionSeparator(
                        new List<IOperator>() {
                            new Operators.ReturnType("out", new Operators.LengthOf(stack, new Operators.Reference(stack, "this")))
                        }
                    )
                , Function))
            );

             String.Object.Add(new Values.Variable("lowercase", new Values.FunctionLiteral(
                stack, new List<string>(), new Operators.ExpressionSeparator(
                    new List<IOperator>() {
                        new Operators.ReturnType("out", new Operators.AnyOperator(
                            new List<IOperator>() { new Operators.Reference(stack, "this") }, 
                            (List<IOperator> args) => {
                                return new Values.StringLiteral(args[0].Run().String.ToLower(), String);
                            }, (List<IOperator> args) => {
                                return $"({args[0].Print()} to lowercase)";
                            }
                        ))
                    }
                )
            , Function)));

            String.Object.Add(new Values.Variable("uppercase", new Values.FunctionLiteral(
                stack, new List<string>(), new Operators.ExpressionSeparator(
                    new List<IOperator>() {
                        new Operators.ReturnType("out", new Operators.AnyOperator(
                            new List<IOperator>() { new Operators.Reference(stack, "this") }, 
                            (List<IOperator> args) => {
                                return new Values.StringLiteral(args[0].Run().String.ToUpper(), String);
                            }, (List<IOperator> args) => {
                                return $"({args[0].Print()} to uppercase)";
                            }
                        ))
                    }
                )
            , Function)));

            Array.Object.Add(new Values.Variable("push", new Values.FunctionLiteral(
                    stack, new List<string>() { "input" }, new Operators.ExpressionSeparator(
                        new List<IOperator>() {
                            new Operators.ReturnType("out", new Operators.Push(new Operators.Reference(stack, "this"), new Operators.Reference(stack, "input")))
                        }
                    )
                , Function)
            ));

            Array.Object.Add(new Values.Variable("pop", new Values.FunctionLiteral(
                    stack, new List<string>(), new Operators.ExpressionSeparator(
                        new List<IOperator>() {
                            new Operators.ReturnType("out", new Operators.Pop(new Operators.Reference(stack, "this"), new Operators.Subtract(stack, new Operators.LengthOf(stack, new Operators.Reference(stack, "this")), new Operators.Number(stack, 1.0))))
                        }
                    )
                , Function)
            ));

            Array.Object.Add(new Values.Variable("remove", new Values.FunctionLiteral(
                    stack, new List<string>() { "index" }, new Operators.ExpressionSeparator(
                        new List<IOperator>() {
                            new Operators.ReturnType("out", new Operators.Pop(new Operators.Reference(stack, "this"), new Operators.Reference(stack, "index")))
                        }
                    )
                , Function)
            ));

            Array.Object.Add(new Values.Variable("length", new Values.FunctionLiteral(
                stack, new List<string>(), new Operators.ExpressionSeparator(
                    new List<IOperator>() {
                        new Operators.ReturnType("out", new Operators.LengthOf(stack, new Operators.Reference(stack, "this")))
                    }
                ), Function
            )));

            top.Add(new Values.Variable("output", new Values.FunctionLiteral(stack, 
                        new List<string>() { "input" }, new Operators.ExpressionSeparator(
                            new List<IOperator>() {
                                new Operators.ReturnType("out", new Operators.Output(
                                        new Operators.Reference(stack, "input")
                                )
                            )
                        }
                    )
                , Function)
            ));
        }
    }
}