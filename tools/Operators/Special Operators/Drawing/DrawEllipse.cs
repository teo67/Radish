namespace Tools.Operators {
    class DrawEllipse : BMPEditor {
        public DrawEllipse(Librarian librarian) : base("drawEllipse", 8, librarian) {}
        protected override void RunBMP(byte[] map, int startIndex, int width, int height, int bpp, int rowLength, Stack Stack) {
            double x = GetArgument(1)._Run(Stack).Number;
            double y = GetArgument(2)._Run(Stack).Number;
            double w = GetArgument(3)._Run(Stack).Number;
            double h = GetArgument(4)._Run(Stack).Number;
            double startAngle = GetArgument(7)._Run(Stack).Number % (2 * Math.PI);
            double endAngle = GetArgument(8)._Run(Stack).Number % (2 * Math.PI);
            IValue color = GetArgument(5)._Run(Stack).Var;
            bool fill = GetArgument(6)._Run(Stack).Boolean;
            List<(int, int)> poses = new List<(int, int)>();
            (bool, bool) isRights = (startAngle < (Math.PI / 2) || startAngle > (3 * Math.PI / 2), (endAngle < (Math.PI / 2) || endAngle > (3 * Math.PI / 2)));         
            (double, double) bottomXs = (startAngle < Math.PI && endAngle < Math.PI && startAngle < endAngle) ? (0, -1) : 
            (x + (startAngle < Math.PI ? -1 : Math.Cos(startAngle)) * w/2, x + (endAngle < Math.PI ? 1 : Math.Cos(endAngle)) * w/2);
            (double, double) topXs = (startAngle > Math.PI && endAngle > Math.PI && startAngle < endAngle) ? (0, -1) : 
            (x + (endAngle > Math.PI ? -1 : Math.Cos(endAngle)) * w/2, x + (startAngle > Math.PI ? 1 : Math.Cos(startAngle)) * w/2);
            (double, double) rightYs = (!isRights.Item1 && !isRights.Item2 && startAngle < endAngle) ? (0, -1) : 
            (y - (!isRights.Item2 ? 1 : Math.Sin(endAngle)) * h/2, y - (!isRights.Item1 ? -1 : Math.Sin(startAngle)) * h/2);
            (double, double) leftYs = (isRights.Item1 && isRights.Item2 && (startAngle < Math.PI ? startAngle + 2 * Math.PI : startAngle) < (endAngle < Math.PI ? endAngle + 2 * Math.PI : endAngle)) ? (0, -1) : 
            (y - (isRights.Item1 ? 1 : Math.Sin(startAngle)) * h/2, y - (isRights.Item2 ? -1 : Math.Sin(endAngle)) * h/2);

            Func<int, double> getbx = (int cx) => {
                double sqrter = 1 - Math.Pow(cx - x, 2.0)/Math.Pow((w/2), 2.0);
                return sqrter < 0 ? y : ((h/2) * Math.Sqrt(sqrter)) + y;
            };

            if(bottomXs.Item1 < bottomXs.Item2) {
                GetEquation(bottomXs.Item1, bottomXs.Item2, y - h/2, y + h/2, poses, getbx, null);
            } else if(bottomXs.Item2 != -1) {
                GetEquation(x - w/2, bottomXs.Item2, y - h/2, y + h/2, poses, getbx, null);
                GetEquation(bottomXs.Item1, x + w/2, y - h/2, y + h/2, poses, getbx, null);
            }
            
            Func<int, double> getry = (int cy) => {
                double sqrter = 1 - Math.Pow(cy - y, 2.0)/Math.Pow((h/2), 2.0);
                return sqrter < 0 ? x : (((w/2) * Math.Sqrt(sqrter)) + x);
            };

            if(rightYs.Item1 < rightYs.Item2) {
                GetEquation(x - w/2, x + w/2, rightYs.Item1, rightYs.Item2, poses, null, getry);
            } else if(rightYs.Item2 != -1) {
                GetEquation(x - w/2, x + w/2, y - h/2, rightYs.Item2, poses, null, getry);
                GetEquation(x - w/2, x + w/2, rightYs.Item1, y + h/2, poses, null, getry);
            }
            
            Func<int, double> gettx = (int cx) => {
                double sqrter = 1 - Math.Pow(cx - x, 2.0)/Math.Pow((w/2), 2.0);
                return sqrter < 0 ? y : ((h/2) * -Math.Sqrt(sqrter)) + y;
            };

            if(topXs.Item1 < topXs.Item2) {
                GetEquation(topXs.Item1, topXs.Item2, y - h/2, y + h/2, poses, gettx, null);
            } else if(topXs.Item2 != -1) {
                GetEquation(x - w/2, topXs.Item2, y - h/2, y + h/2, poses, gettx, null);
                GetEquation(topXs.Item1, x + w/2, y - h/2, y + h/2, poses, gettx, null);
            }

            Func<int, double> getly = (int cy) => {
                double sqrter = 1 - Math.Pow(cy - y, 2.0)/Math.Pow((h/2), 2.0);
                return sqrter < 0 ? x : ((w/2) * -Math.Sqrt(sqrter)) + x;
            };

            if(leftYs.Item1 < leftYs.Item2) {
                GetEquation(x - w/2, x + w/2, leftYs.Item1, leftYs.Item2, poses, null, getly);
            } else if(leftYs.Item2 != -1) {
                GetEquation(x - w/2, x + w/2, y - h/2, leftYs.Item2, poses, null, getly);
                GetEquation(x - w/2, x + w/2, leftYs.Item1, y + h/2, poses, null, getly);
            }
            
            if(fill) {
                double y1 = -(h/2) * Math.Sin(startAngle) + y;
                double y2 = -(h/2) * Math.Sin(endAngle) + y;
                double x1 = (w/2) * Math.Cos(startAngle) + x;
                double x2 = (w/2) * Math.Cos(endAngle) + x;

                double m = y2 == y1 ? 0 : ((x2 - x1) / (y2 - y1));
                foreach((int, int) pose in poses) {
                    int pointOnLine = y2 == y1 ? (int)Math.Round(x) : ((int)Math.Round((pose.Item2 - y1) * m + x1));
                    double maxMagnitude = (w/2) * Math.Sqrt(1 - Math.Pow(pose.Item2 - y, 2.0)/Math.Pow(h/2, 2.0));
                    if(pointOnLine > x + maxMagnitude) {
                        pointOnLine = (int)Math.Round(x + maxMagnitude);
                    }
                    if(pointOnLine < x - maxMagnitude) {
                        pointOnLine = (int)Math.Round(x - maxMagnitude);
                    }
                    int minx = Math.Min(pose.Item1, pointOnLine);
                    EditPixel(map, startIndex + (height - pose.Item2 - 1) * rowLength, minx, pose.Item2, width, bpp, color, (minx == pose.Item1 ? pointOnLine : pose.Item1) - minx + 1);
                }
            } else {
                foreach((int, int) pos in poses) {
                    EditPixel(map, startIndex + (height - pos.Item2 - 1) * rowLength, pos.Item1, pos.Item2, width, bpp, color);
                }
            }
        }
    }
}