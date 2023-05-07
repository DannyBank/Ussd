using System;

namespace Dbank.Digisoft.Ussd.Data.Extensions
{
    public static class DoubleExtensions
    {
        static double[] pow10 = { 1e0, 1e1, 1e2, 1e3, 1e4, 1e5, 1e6, 1e7, 1e8, 1e9, 1e10 };
        public static double Truncate(this double x, int precision)
        {
            if (precision < 0)
                throw new ArgumentException();
            if (precision == 0)
                return Math.Truncate(x);
            double m = precision >= pow10.Length ? Math.Pow(10, precision) : pow10[precision];
            return Math.Truncate(x * m) / m;
        }
       
    }
}
