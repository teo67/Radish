using System.Text;
namespace Tools.Operators {
    class GenerateQR : Operator {
        private IOperator SourceString { get; }
        private IOperator TargetString { get; }
        public GenerateQR(IOperator source, IOperator target) : base(-1, -1) {
            this.SourceString = source;
            this.TargetString = target;
        }
        private static int[] Capacities { get; }
        static GenerateQR() {
            Capacities = new int[] {
                0, 11, 20, 32, 46, 60, 74, 86, 108, 130,    /* <-- 2 bytes pad (0-9), 3 bytes pad --> */    151, 177, 203, 241, 258, 292, 322, 364, 394, 442, 482, 509, 565, 611, 661, 715, 751, 805, 868, 908, 982, 1030, 1112, 1168, 1228, 1283, 1351, 1423, 1499, 1579, 1663
            };
        }
        private void GetQR(byte[] full, int start, int numQR = 0) {
            int index = -1;
            for(int i = 0; i < Capacities.Length; i++) {
                if(i > 0 && Capacities[i] <= full.Length - start) {
                    index = i;
                    break;
                }
            }
            if(index == -1) {
                index = Capacities.Length - 1;
                GetQR(full, start + Capacities[Capacities.Length - 1], numQR + 1);
            }
            string content = "0100";
            int charcount = Encoding.UTF8.GetDecoder().GetCharCount(full, start, Math.Min(full.Length - start, Capacities[index]));
            content += Convert.ToString(charcount, 2).PadLeft(index < 10 ? 8 : 16); // character indicator bit value
            for(int i = start; i < start + Capacities[index]; i++) {
                if(i >= full.Length) {
                    continue;
                }   
                content += Convert.ToString(full[i], 2).PadLeft(8, '0');
            }
            int withpad = 8 * (Capacities[index] + (index < 10 ? 2 : 3));
            if(content.Length < withpad) {
                content += new string('0', Math.Min(4, withpad - content.Length));
            }
            if(content.Length % 8 != 0) {
                content += new string('0', 8 - (content.Length % 8));
            }
            while(content.Length < withpad) {
                content += "11101100";
                if(content.Length >= withpad) {
                    break;
                }
                content += "00010001";
            }
            
        }
        public override IValue Run(Stack Stack)
        {
            
            // byte[] res = Encoding.UTF8.GetBytes(SourceString._Run(Stack).String);
            // string total = "0100"; // byte mode
            // int max = 1663;
            // res[0].
            // return new Values.NoneLiteral();
        }
        public override string Print() {
            return $"generateQR({SourceString.Print()}, {TargetString.Print()})";
        }
    }
}