// namespace Tools.Operators {
//     class ObjectFromClass : SimpleOperator {
//         private Stack Stack { get; }
//         public ObjectFromClass(IOperator left, IOperator right, Stack stack) : base(left, right, "from class") {
//             this.Stack = stack;
//         }
//         public override IValue Run() {
//             Class _class = Left.Run().Class;
//             Stack.Push();
//             for(int i = 0; i < _class.Properties.Count; i++) {
//                 Stack.Head.Val.Add(new Values.Variable(_class.Properties[i], _class.Values[i]));
//             }
//             Left.Run().Class.Constructor.Function(Right.Run().Array);
//             return new Values.ObjectLiteral(Stack.Pop().Val);
//         }
//     }
// }