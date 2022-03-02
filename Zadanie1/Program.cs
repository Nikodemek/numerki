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
        private static void Main()
        {
            Func<double, double> exprLinear = x => x*x - 2;
            Func<double, double> deriLinear = x => 2 * x;
            string exprLinearString = "x^2 - 2";
            double epsilon = 0.000001;
            int iterations = 7;

            double zeroLinear;

            zeroLinear = FindZeroArgBisection(exprLinear, -1.0, 5.0, epsilon);
            LogResult(exprLinear, exprLinearString, zeroLinear, epsilon, "Bisection");
            zeroLinear = FindZeroArgNewtons(exprLinear, deriLinear, -1.0, 5.0, epsilon);
            LogResult(exprLinear, exprLinearString, zeroLinear, epsilon, "Newton's");

            zeroLinear = FindZeroArgBisection(exprLinear, -1.0, 5.0, iterations);
            LogResult(exprLinear, exprLinearString, zeroLinear, iterations, "Bisection");
            zeroLinear = FindZeroArgNewtons(exprLinear, deriLinear, -1.0, 5.0, iterations);
            LogResult(exprLinear, exprLinearString, zeroLinear, iterations, "Newton's");

            Console.ReadKey();
        }

        #region Bisection method

        private static double FindZeroArgBisection(Func<double, double> expr, double min, double max, double eps)
        {
            if (expr(min) * expr(max) > 0 || eps <= 0.0) throw new ArgumentException("Złe argumenty");

            double prevPotentialZero = max;
            double upperBound = max;
            double lowerBound = min;
            double potentialZero = (upperBound + lowerBound) * 0.5;

            while (Math.Abs(prevPotentialZero - potentialZero) > eps)
            {
                double result = expr(potentialZero);

                if (result < 0) lowerBound = potentialZero;
                else upperBound = potentialZero;

                prevPotentialZero = potentialZero;
                potentialZero = (upperBound + lowerBound) * 0.5;
            }

            return potentialZero;
        }

        private static double FindZeroArgBisection(Func<double, double> expr, double min, double max, int iterations)
        {
            if (expr(min) * expr(max) > 0 || iterations <= 0) throw new ArgumentException("Zły argument");

            double upperBound = max;
            double lowerBound = min;
            double potentialZero = (upperBound + lowerBound) * 0.5;

            for (var i = 0; i < iterations - 1; i++)
            {
                double result = expr(potentialZero);

                if (result < 0) lowerBound = potentialZero;
                else upperBound = potentialZero;

                potentialZero = (upperBound + lowerBound) * 0.5;
            }

            return potentialZero;
        }

        #endregion

        #region Newton's method

        private static double FindZeroArgNewtons(Func<double, double> expr, Func<double, double> deriv, double min, double max, double eps)
        {
            if (expr(min) * expr(max) > 0 || eps <= 0.0) throw new ArgumentException("Złe argumenty");

            double x = max;
            double potentialZero = x - expr(x) / deriv(x);

            while (Math.Abs(potentialZero - x) > eps)
            {
                x = potentialZero;
                potentialZero = x - expr(x) / deriv(x);
            }

            return potentialZero;
        }

        private static double FindZeroArgNewtons(Func<double, double> expr, Func<double, double> deriv, double min, double max, int iterations)
        {
            if (expr(min) * expr(max) > 0 || iterations <= 0) throw new ArgumentException("Złe argumenty");

            double x = max;
            double potentialZero = x - expr(x) / deriv(x);

            for (var i = 0; i < iterations - 1; i++)
            {
                x = potentialZero;
                potentialZero = x - expr(x) / deriv(x);
            }

            return potentialZero;
        }

        #endregion

        #region Utils

        private static void LogResult(Func<double, double> expression, string function, double root, double epsilon, string method)
        {
            Console.WriteLine($"Function f(x) = {function} is zero when x = {root}\n(calculated using {method} method, with a precision of {epsilon}).\nf({root}) = {expression(root):n20}\n");
        }

        private static void LogResult(Func<double, double> expression, string function, double root, int iterations, string method)
        {
            Console.WriteLine($"Function f(x) = {function} is zero when x = {root}\n(calculated using {method} method, using {iterations} iterations).\n(f({root}) = {expression(root):n20})\n");
        }

        #endregion
    }
}
