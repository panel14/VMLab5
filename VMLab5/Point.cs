using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMLab5
{
    internal class Point
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;

        }

        public Point()
        {
            X = 0;
            Y = 0;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}
