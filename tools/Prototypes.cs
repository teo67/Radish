namespace Tools { // adds basic prototypes to call stack
    class Prototypes {
        public Prototypes(Stack stack) {
            List<Values.Variable> top = stack.Head.Val;
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
                    stack, new List<string>(), new List<IOperator?>(), new Operators.ExpressionSeparator(-1, -1, 
                        new List<IOperator>() {
                            new Operators.ReturnType("harvest", -1, -1, new Operators.Reference(stack, "this", -1, -1))
                        }
                    )
            , Function)));

            String.Object.Add(new Values.Property("length", new Values.FunctionLiteral(
                    stack, new List<string>(), new List<IOperator?>(), new Operators.ExpressionSeparator(-1, -1,
                        new List<IOperator>() {
                            new Operators.ReturnType("harvest", -1, -1, new Operators.LengthOf(stack, new Operators.Reference(stack, "this", -1, -1), -1, -1))
                        }
                    )
                , Function), null)
            );

             String.Object.Add(new Values.Property("lowercase", new Values.FunctionLiteral(
                stack, new List<string>(), new List<IOperator?>(), new Operators.ExpressionSeparator(-1, -1,
                    new List<IOperator>() {
                        new Operators.ReturnType("harvest", -1, -1, new Operators.AnyOperator(
                            new List<IOperator>() { new Operators.Reference(stack, "this", -1, -1) }, 
                            (List<IOperator> args) => {
                                return new Values.StringLiteral(args[0]._Run().String.ToLower(), String);
                            }, (List<IOperator> args) => {
                                return $"({args[0].Print()} to lowercase)";
                            }
                        ))
                    }
                )
            , Function), null));

            String.Object.Add(new Values.Property("uppercase", new Values.FunctionLiteral(
                stack, new List<string>(), new List<IOperator?>(), new Operators.ExpressionSeparator(-1, -1,
                    new List<IOperator>() {
                        new Operators.ReturnType("harvest", -1, -1, new Operators.AnyOperator(
                            new List<IOperator>() { new Operators.Reference(stack, "this", -1, -1) }, 
                            (List<IOperator> args) => {
                                return new Values.StringLiteral(args[0]._Run().String.ToUpper(), String);
                            }, (List<IOperator> args) => {
                                return $"({args[0].Print()} to uppercase)";
                            }
                        ))
                    }
                )
            , Function), null));

            Array.Object.Add(new Values.Variable("push", new Values.FunctionLiteral(
                    stack, new List<string>() { "input" }, new List<IOperator?>() { null }, new Operators.ExpressionSeparator(-1, -1,
                        new List<IOperator>() {
                            new Operators.ReturnType("harvest", -1, -1, new Operators.Push(new Operators.Reference(stack, "this", -1, -1), new Operators.Reference(stack, "input", -1, -1), -1, -1))
                        }
                    )
                , Function)
            ));

            Array.Object.Add(new Values.Variable("pop", new Values.FunctionLiteral(
                    stack, new List<string>(), new List<IOperator?>(), new Operators.ExpressionSeparator(-1, -1,
                        new List<IOperator>() {
                            new Operators.ReturnType("harvest", -1, -1, new Operators.Pop(new Operators.Reference(stack, "this", -1, -1), new Operators.Subtract(stack, new Operators.LengthOf(stack, new Operators.Reference(stack, "this", -1, -1), -1, -1), new Operators.Number(stack, 1.0, -1, -1), -1, -1), -1, -1))
                        }
                    )
                , Function)
            ));

            Array.Object.Add(new Values.Variable("remove", new Values.FunctionLiteral(
                    stack, new List<string>() { "index" }, new List<IOperator?>() { null }, new Operators.ExpressionSeparator(-1, -1,
                        new List<IOperator>() {
                            new Operators.ReturnType("harvest", -1, -1, new Operators.Pop(new Operators.Reference(stack, "this", -1, -1), new Operators.Reference(stack, "index", -1, -1), -1, -1))
                        }
                    )
                , Function)
            ));

            Array.Object.Add(new Values.Property("length", new Values.FunctionLiteral(
                stack, new List<string>(), new List<IOperator?>(), new Operators.ExpressionSeparator(-1, -1,
                    new List<IOperator>() {
                        new Operators.ReturnType("harvest", -1, -1, new Operators.LengthOf(stack, new Operators.Reference(stack, "this", -1, -1), -1, -1))
                    }
                ), Function
            ), null));

            top.Add(new Values.Variable("holler", new Values.FunctionLiteral(stack, 
                        new List<string>() { "input" }, new List<IOperator?>() { null }, new Operators.ExpressionSeparator(-1, -1,
                            new List<IOperator>() {
                                new Operators.ReturnType("harvest", -1, -1, new Operators.Output(
                                        new Operators.Reference(stack, "input", -1, -1), -1, -1
                                )
                            )
                        }
                    )
                , Function)
            ));

            top.Add(new Values.Variable("wait", new Values.FunctionLiteral(stack, 
                        new List<string>() { "input" }, new List<IOperator?>() { null }, new Operators.ExpressionSeparator(-1, -1,
                            new List<IOperator>() {
                                new Operators.Wait(new Operators.Reference(stack, "input", -1, -1), -1, -1)
                            }), Function)));
        }
    }
}