using System;

/*                           ''        
    '||''|, .|''|, `||''|,   ||  ('''' 
     ||  || ||..||  ||  ||   ||   `'') 
     ||..|' `|...  .||  ||. .||. `...' 
     ||                                
    .|| 
*/

namespace Zadanie1
{
    class Program
    {
        private static void Main(string[] args)
        {
            Func<double, double> exprLinear = x => x*x - 2;
            Func<double, double> deriLinear = x => 2 * x;

            double zeroLinear = FindZeroArgBisect(exprLinear, -1.0, 5.0, 0.000001);
            Console.WriteLine($"Function y = 3x + 2.5 has a zero with x = {zeroLinear}, (y({zeroLinear}) = {exprLinear(zeroLinear):n7})");
            zeroLinear = FindZeroArgIncisal(exprLinear, deriLinear, -1.0, 5.0, 0.01);
            Console.WriteLine($"Function y = 3x + 2.5 has a zero with x = {zeroLinear}, (y({zeroLinear}) = {exprLinear(zeroLinear):n7})");

            zeroLinear = FindZeroArgBisect(exprLinear, -1.0, 5.0, 9);
            Console.WriteLine($"Function y = 3x + 2.5 has a zero with x = {zeroLinear}, (y({zeroLinear}) = {exprLinear(zeroLinear):n7})");
            zeroLinear = FindZeroArgIncisal(exprLinear, deriLinear, -1.0, 5.0, 1);
            Console.WriteLine($"Function y = 3x + 2.5 has a zero with x = {zeroLinear}, (y({zeroLinear}) = {exprLinear(zeroLinear):n7})");

            Console.ReadKey();
        }

        // -------------------- Bisect --------------------

        private static double FindZeroArgBisect(Func<double, double> expr, double min, double max, double eps)
        {
            if (expr(min) * expr(max) > 0) throw new ArgumentException("Zły argument");

            double prevPotentialZero = max;
            double upperBound = max;
            double lowerBound = min;
            double potentialZero = (upperBound + lowerBound) * 0.5;

            while (Math.Abs(prevPotentialZero - potentialZero) > eps)
            {
                double result = expr(potentialZero);

                if (result < 0)
                {
                    lowerBound = potentialZero;
                }
                else
                {
                    upperBound = potentialZero;
                }

                prevPotentialZero = potentialZero;
                potentialZero = (upperBound + lowerBound) * 0.5;
            }

            return potentialZero;
        }

        private static double FindZeroArgBisect(Func<double, double> expr, double min, double max, int iterations)
        {
            if (expr(min) * expr(max) > 0 
                || iterations == 0) throw new ArgumentException("Zły argument");

            double upperBound = max;
            double lowerBound = min;
            double potentialZero = (upperBound + lowerBound) * 0.5;

            for (var i = 0; i < iterations - 1; i++)
            {
                double result = expr(potentialZero);

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

            return potentialZero;
        }

        // -------------------- Incisal --------------------

        private static double FindZeroArgIncisal(Func<double, double> expr, Func<double, double> deri, double min, double max, double eps)
        {
            if (expr(min) * expr(max) > 0) throw new ArgumentException("Zły argument");

            double x = min;
            double potentialZero = x - expr(x) / deri(x);

            while (Math.Abs(potentialZero - x) > eps)
            {
                x = potentialZero;
                potentialZero = x - expr(x) / deri(x);
            }

            return potentialZero;
        }

        private static double FindZeroArgIncisal(Func<double, double> expr, Func<double, double> deri, double min, double max, int iterations)
        {
            if (expr(min) * expr(max) > 0 
                || iterations == 0) throw new ArgumentException("Zły argument");

            double x = min;
            double potentialZero = x - expr(x) / deri(x);

            for (var i = 0; i < iterations - 1; i++)
            {
                x = potentialZero;
                potentialZero = x - expr(x) / deri(x);
            }

            return potentialZero;
        }

    }
}
