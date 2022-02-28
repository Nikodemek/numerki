using System;

namespace Zadanie1
{
    class Program
    {
        private static void Main(string[] args)
        {
            Func<double, double> exprLinear = x => 3 * x + 2.5;
            double zeroLinear = FindZeroArgBisect(exprLinear, 2.0, 5.0, 0.000001);
            Console.WriteLine($"Function y = 3x + 2.5 has a zero with x = {zeroLinear}, (y({zeroLinear}) = {exprLinear(zeroLinear):n7})");

            Console.ReadKey();
        }

        private static double FindZeroArgBisect(Func<double, double> expr, double min, double max, double eps)
        {
            if (expr(min) * expr(max) > 0) throw new ArgumentException("Zły argument");

            double upperBound = max;
            double lowerBound = min;
            double potentialZero = (upperBound + lowerBound) * 0.5;
            double result;

            do
            {
                result = expr(potentialZero);

                if (result < 0)
                {
                    lowerBound = potentialZero;
                }
                else
                {
                    upperBound = potentialZero;
                }
                
                potentialZero = (upperBound + lowerBound) * 0.5;
            }
            while (Math.Abs(result) > eps);

            return potentialZero;
        }
    }
}
