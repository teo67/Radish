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

            String.Object.Add(new Values.Variable("lowercase", new Values.FunctionLiteral( // add this after bc it refers to string
                    stack, new List<string>(), new Operators.ExpressionSeparator(
                        new List<IOperator>() {
                            new Operators.ReturnType("out", new Operators.Lowercase(stack, new Operators.Reference(stack, "this")))
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