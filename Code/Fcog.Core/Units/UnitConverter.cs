using System;

namespace Fcog.Core.Units
{
    public static class UnitConverter
    {
        private const double mmIninch = 25.4;

        public static double PxToMm(int px, double resolution)
        {
            if (resolution < double.Epsilon)
            {
                throw new ArgumentOutOfRangeException(nameof(resolution));
            }

            var mm = px / resolution * mmIninch;
            return mm;
        }


        public static int MmToPx(double mm, double resolution)
        {
            var dpi = mm * resolution / mmIninch;
            return (int) Math.Round(dpi, 0);
        }
    }
}