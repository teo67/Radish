// using IronBarCode;
// namespace Tools.Operators {
//     class ReadQR : Operator {
//         private IOperator SourceString { get; }
//         public ReadQR(IOperator source, int row, int col) : base(row, col) {
//             this.SourceString = source;
//         }
//         public override IValue Run(Stack Stack)
//         {
//             BarcodeResults res = BarcodeReader.Read(SourceString._Run(Stack).String);
//             QRCodeWriter.CreateQrCode()
//         }
//     }
// }