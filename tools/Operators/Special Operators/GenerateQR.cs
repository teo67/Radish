using QRCoder;
using System.Text;
using ZXing;
namespace Tools.Operators {
    class GenerateQR : Operator {
        private IOperator SourceString { get; }
        private IOperator TargetString { get; }
        public GenerateQR(IOperator source, IOperator target) : base(-1, -1) {
            this.SourceString = source;
            this.TargetString = target;
        }
        public override IValue Run(Stack Stack)
        {
            int max = 1663;
            string target = TargetString._Run(Stack).String;
            string withoutExtension = target.EndsWith(".png") ? target.Substring(0, target.Length - 4) : target;
            byte[] res1 = Encoding.UTF8.GetBytes(SourceString._Run(Stack).String);
            QRCodeGenerator gen = new QRCodeGenerator();
            for(int i = 0; i < res1.Length; i += max) {
                byte[] part = new byte[max];
                Array.Copy(res1, i, part, 0, Math.Min(res1.Length - i, max));
                QRCodeData res = gen.CreateQrCode(part, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode final = new PngByteQRCode(res);
                File.WriteAllBytes(i == 0 ? withoutExtension + ".png" : $"{withoutExtension} ({i / max}).png", final.GetGraphic(20));
            }
            return new Values.NoneLiteral();
        }
        public override string Print() {
            return $"generateQR({SourceString.Print()}, {TargetString.Print()})";
        }
    }
}