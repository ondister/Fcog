using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Point = Accord.Point;

namespace Fcog.Core.Math
{
   
    public static class Geometry
    {
        public static double PxToMm(int px, double resolution)
        {
           
            double mm = 0;
            mm = px / resolution * 25.4;
            return mm;
        }



        public static int MmToPx(double mm, double resolution)
        {
            double dpi = 0;
             dpi = mm * resolution / 25.4;
            return (int)dpi;
        }

        public static double EuclidianDistance(Point point1, Point point2)
        {
            var distance = 0.0d;
            distance = System.Math.Sqrt(System.Math.Pow(point1.X - point2.X, 2) + System.Math.Pow(point1.Y - point2.Y, 2));
            return distance;

        }
    }
}
