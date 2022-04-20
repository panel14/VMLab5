using System;
namespace VMLab5
{
    internal class Solver
    {
        private static readonly int ApproximationsNumber = 4;

        private static Func<Point, double> function1 = (Point) => Math.Pow(Point.X, 2) - 2 * Point.Y;
        private static Func<Point, double> function2 = (Point) => Point.X * Math.Pow(Point.Y, 2);
        private static Func<Point, double> function3 = (Point) =>
        {
            if (Point.X == 0) Point.X = 0.0001;
            return Point.Y * 3 / Point.X + Math.Pow(Point.X, 3) + Point.X;
        };

        private static Func<double, double, double> solution1 = (x, C) => (C / Math.Pow(Math.E, 2 * x)) + (Math.Pow(x, 2) / 2) - (x / 2) + 0.25;
        private static Func<double, double, double> solution2 = (x, C) => -2 / (Math.Pow(x, 2) + C);
        private static Func<double, double, double> solution3 = (x, C) => Math.Pow(x, 4) - Math.Pow(x, 2) + C * Math.Pow(Math.Abs(x), 3);

        private static Func<Point, double> coefFunction1 = (Point) => Math.Pow(Math.E, 2 * Point.X) * (Point.Y - Math.Pow(Point.X, 2) / 2 + Point.X / 2 - 0.25);
        private static Func<Point, double> coefFunction2 = (Point) => -2 / Point.Y - Math.Pow(Point.X, 2);
        private static Func<Point, double> coefFunction3 = (Point) => (Point.Y - Math.Pow(Point.X, 4) + Math.Pow(Point.X, 2)) / Math.Pow(Math.Abs(Point.X), 3);

        private static Func<Point, double>[] functions = { function1, function2, function3 };
        private static Func<double, double, double>[] solutions = { solution1, solution2, solution3 };
        private static Func<Point, double>[] coefFunctions = { coefFunction1, coefFunction2, coefFunction3 };

        private static Func<Point[], Point[]>[] approximations = { ApproximateLinear, ApproximateExponential, ApproximateLogarithmic, ApproximatePolinomical };

        private static double GetH(double[] borders, int splitsNumber)
        {
            return (Math.Abs(borders[0]) + Math.Abs(borders[1])) / splitsNumber;
        }

        public static Point[] SolveByImprovedEulerMethod(double[] borders, int functionNumber, int splitsNumber, Point firstPoint)
        {
            double h = GetH(borders, splitsNumber);

            Point[] calculatedPoints = new Point[splitsNumber];
            calculatedPoints[0] = firstPoint;

            Point currentPoint = firstPoint;

            for (int i = 1; i < splitsNumber; i++)
            {
                double firstDeltaArg = currentPoint.X + h / 2;
                double secondDeltaArg = currentPoint.Y + h / 2 * functions[functionNumber](currentPoint);

                double delta = h * functions[functionNumber](new Point(firstDeltaArg, secondDeltaArg));

                double newY = currentPoint.Y + delta;

                currentPoint = new Point(currentPoint.X + h, newY);

                calculatedPoints[i] = currentPoint;
            }

            return calculatedPoints;
        }

        public static double GetCFromInitConditions(Point initPoint, int equationNumber)
        {
            return coefFunctions[equationNumber](initPoint);
        }

        public static Point[] GetAnaliiticSolution(double[] borders, int solutionNumber, int splitsNumber, Point firstPoint)
        {
            Point[] solutionPoints = new Point[splitsNumber];

            double h = GetH(borders, splitsNumber);
            double C = GetCFromInitConditions(firstPoint, solutionNumber);

            double currentX = firstPoint.X;
            solutionPoints[0] = firstPoint;

            for (int i = 1; i < splitsNumber; i++)
            {
                currentX += h;
                double currentY = solutions[solutionNumber](currentX, C);

                solutionPoints[i] = new Point(currentX, currentY);
            }

            return solutionPoints;
        }

        private static double[] GetDets2(double[][] sysMatrix)
        {
            double mainDet = sysMatrix[0][0] * sysMatrix[1][1] - sysMatrix[0][1] * sysMatrix[1][0];
            double aDet = sysMatrix[0][2] * sysMatrix[1][1] - sysMatrix[0][1] * sysMatrix[1][2];
            double bDet = sysMatrix[0][0] * sysMatrix[1][2] - sysMatrix[0][2] * sysMatrix[1][0];

            double aCoef = aDet / mainDet;
            double bCoef = bDet / mainDet;

            return new double[] { aCoef, bCoef };
        }

        private static double[] GetDets3(double[][] sysMatrix)
        {
            double mainDet = sysMatrix[0][0] * sysMatrix[1][1] * sysMatrix[2][2] +
                             sysMatrix[0][2] * sysMatrix[1][0] * sysMatrix[2][1] +
                             sysMatrix[2][0] * sysMatrix[0][1] * sysMatrix[1][2] -

                             sysMatrix[2][0] * sysMatrix[1][1] * sysMatrix[0][2] -
                             sysMatrix[0][0] * sysMatrix[1][2] * sysMatrix[2][1] -
                             sysMatrix[2][2] * sysMatrix[0][1] * sysMatrix[1][0];

            double aDet = sysMatrix[0][3] * sysMatrix[1][1] * sysMatrix[2][2] +
                             sysMatrix[0][2] * sysMatrix[1][3] * sysMatrix[2][1] +
                             sysMatrix[2][3] * sysMatrix[0][1] * sysMatrix[1][2] -

                             sysMatrix[2][3] * sysMatrix[1][1] * sysMatrix[0][2] -
                             sysMatrix[0][3] * sysMatrix[1][2] * sysMatrix[2][1] -
                             sysMatrix[2][2] * sysMatrix[0][1] * sysMatrix[1][3];

            double bDet = sysMatrix[0][0] * sysMatrix[1][3] * sysMatrix[2][2] +
                             sysMatrix[0][2] * sysMatrix[1][0] * sysMatrix[2][3] +
                             sysMatrix[2][0] * sysMatrix[0][3] * sysMatrix[1][2] -

                             sysMatrix[2][0] * sysMatrix[1][3] * sysMatrix[0][2] -
                             sysMatrix[0][0] * sysMatrix[1][2] * sysMatrix[2][3] -
                             sysMatrix[2][2] * sysMatrix[0][3] * sysMatrix[1][0];

            double cDet = sysMatrix[0][0] * sysMatrix[1][1] * sysMatrix[2][3] +
                             sysMatrix[0][3] * sysMatrix[1][0] * sysMatrix[2][1] +
                             sysMatrix[2][0] * sysMatrix[0][1] * sysMatrix[1][3] -

                             sysMatrix[2][0] * sysMatrix[1][1] * sysMatrix[0][3] -
                             sysMatrix[0][0] * sysMatrix[1][3] * sysMatrix[2][1] -
                             sysMatrix[2][3] * sysMatrix[0][1] * sysMatrix[1][0];

            return new double[] { aDet / mainDet, bDet / mainDet, cDet / mainDet };
        }

        private static Point[] ApproximateLinear(Point[] data)
        {
            Point[] points = new Point[data.Length];

            double xSqrtSum = 0; Array.ForEach(data, (x) => xSqrtSum += Math.Pow(x.X, 2));
            double xSum = 0; Array.ForEach(data, (x) => xSum += x.X);
            double multyXY = 0; Array.ForEach(data, (x) => multyXY += x.X * x.Y);
            double ySum = 0; Array.ForEach(data, (x) => ySum += x.Y);

            double n = data.Length;

            double[][] sysMatrix = new double[2][];
            sysMatrix[0] = new double[] { xSqrtSum, xSum, multyXY };
            sysMatrix[1] = new double[] { xSum, n, ySum };

            double[] coefs = GetDets2(sysMatrix);

            double aCoef = coefs[0]; double bCoef = coefs[1];

            for (int i = 0; i < data.Length; i++)
            {
                double y = coefs[0] * data[i].X + coefs[1];
                points[i] = new Point(data[i].X, y);
            }

            return points;
        }

        private static Point[] ApproximateLogarithmic(Point[] data)
        {
            Point[] points = new Point[data.Length];

            double xSLnSum = 0; Array.ForEach(data, (x) =>
            {
                if (x.X > 0)
                    xSLnSum += Math.Pow(Math.Log(x.X), 2);
            });
            
            double xLnSum = 0; Array.ForEach(data, (x) => 
            {
                if (x.X > 0)
                    xLnSum += Math.Log(x.X);
            });
            double multyLnXY = 0; Array.ForEach(data, (x) => 
            {
                if (x.X > 0)
                    multyLnXY += x.Y * Math.Log(x.X);
            });
            double ySum = 0; Array.ForEach(data, (x) => ySum += x.Y);

            double n = data.Length;

            double[][] sysMatrix = new double[2][];
            sysMatrix[0] = new double[] { xSLnSum, xLnSum, multyLnXY };
            sysMatrix[1] = new double[] { xLnSum, n, ySum };

            double[] coefs = GetDets2(sysMatrix);

            double aCoef = coefs[0]; double bCoef = coefs[1];

            for (int i = 0; i < data.Length; i++)
            {
                double y = 0;
                if (data[i].X > 0)
                    y = coefs[0] * Math.Log(data[i].X) + coefs[1];
                points[i] = new Point(data[i].X, y);
            }

            return points;
        }

        private static Point[] ApproximateExponential(Point[] data)
        {
            Point[] points = new Point[data.Length];

            double xSqrtSum = 0; Array.ForEach(data, (x) => xSqrtSum += Math.Pow(x.X, 2));
            double xSum = 0; Array.ForEach(data, (x) => xSum += x.X);
            double multyXLnY = 0; Array.ForEach(data, (x) => {
                if (x.Y > 0)
                    multyXLnY += x.X * Math.Log(x.Y);
            });
            double lnYSum = 0; Array.ForEach(data, (x) => {
                if (x.Y > 0)
                    lnYSum += Math.Log(x.Y);
            });

            double n = data.Length;

            double[][] sysMatrix = new double[2][];
            sysMatrix[0] = new double[] { xSqrtSum, xSum, multyXLnY };
            sysMatrix[1] = new double[] { xSum, n, lnYSum };

            double[] coefs = GetDets2(sysMatrix);

            double aCoef = coefs[0]; double bCoef = Math.Pow(Math.E, coefs[1]);

            for (int i = 0; i < data.Length; i++)
            {
                double y = bCoef * Math.Pow(Math.E, coefs[0] * data[i].X);
                points[i] = new Point(data[i].X, y);
            }

            return points;
        }

        private static Point[] ApproximatePolinomical(Point[] data)
        {
            Point[] points = new Point[data.Length];

            double x4Sum = 0; Array.ForEach(data, (x) => x4Sum += Math.Pow(x.X, 4));
            double x3Sum = 0; Array.ForEach(data, (x) => x3Sum += Math.Pow(x.X, 3));
            double x2Sum = 0; Array.ForEach(data, (x) => x2Sum += Math.Pow(x.X, 2));
            double xSum = 0; Array.ForEach(data, (x) => xSum += x.X);
            double ySum = 0; Array.ForEach(data, (x) => ySum += x.Y);
            double x2ySum = 0; Array.ForEach(data, (x) => x2ySum += Math.Pow(x.X, 2) * x.Y);
            double xySum = 0; Array.ForEach(data, (x) => xySum += x.Y * x.X);

            double n = data.Length;

            double[][] sysMatrix = new double[3][];
            sysMatrix[0] = new double[] { x4Sum, x3Sum, x2Sum, x2ySum };
            sysMatrix[1] = new double[] { x3Sum, x2Sum, xSum, xySum };
            sysMatrix[2] = new double[] { x2Sum, xSum, n, ySum };

            double[] coefs = GetDets3(sysMatrix);

            for (int i = 0; i < data.Length; i++)
            {
                double y = coefs[0] * Math.Pow(data[i].X, 2) + coefs[1] * data[i].X + coefs[2];
                points[i] = new Point(data[i].X, y);
            }

            return points;
        }

        private static double[] CalculateReliabilitys(Point[][] points, Point[] mainData)
        {
            double[] reliabilitys = new double[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                double errorsSum = 0;
                double approximaSqr = 0;
                double approxSum = 0;
                
                for (int j = 0; j < points[i].Length; j++)
                {
                    errorsSum += Math.Pow(mainData[j].Y - points[i][j].Y, 2);
                    approximaSqr += Math.Pow(points[i][j].Y, 2);
                    approxSum += points[i][j].Y;
                }
                reliabilitys[i] = 1 - errorsSum / (approximaSqr - 1.0 / points[i].Length * Math.Pow(approxSum, 2));
            }

            return reliabilitys;
        }

        private static int GetMaxReliability(double[] reliabilitys)
        {
            double max = Double.MinValue;
            int index = reliabilitys.Length;

            for (int i = 0; i < reliabilitys.Length; i++)
            {
                if (reliabilitys[i] > max)
                {
                    max = reliabilitys[i];
                    index = i;
                } 
            }
            return index;
        }

        public static Point[] Approximate(Point[] data)
        {
            Point[][] approximatedPoints = new Point[ApproximationsNumber][];

            for (int i = 0; i < ApproximationsNumber; i++)
                approximatedPoints[i] = approximations[i](data);

            double[] reliabilitys = CalculateReliabilitys(approximatedPoints, data);

            return approximatedPoints[GetMaxReliability(reliabilitys)];
        }
    }

}
