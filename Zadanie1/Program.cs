using System;
using System.Runtime.CompilerServices;

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
            Func<double, double> exprLinear = x => x*x*x - 2*x - 5;
            Func<double, double> deriLinear = x => 3 * x * x - 2;
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

            MiniMenu();

            Console.ReadKey();
        }

        private static void MiniMenu()
        {
            Func<double, double> exprLinear = null, deriLinear = null;
            int a, b;
            double epsilon;
            int iterations;
            string exprLinearString = null;
            double zeroLinear;
            int choice;

            Console.WriteLine("Which nonlinear function do you want to calculate the zero point?");
            Console.WriteLine("1. f(x) = x^2 - 2");
            Console.WriteLine("2. f(x) = x^3 - 2x - 5");
            Console.WriteLine("3. tan(x - 3)");
            Console.WriteLine("4. 2^x - 3");
            Console.WriteLine("5. tan(2^x - 3)");
            Console.WriteLine("6. sin(x^2 - 2)");
            Console.WriteLine("7. 2^sin(x) - 1");

            bool uncorrect = true;

            while (uncorrect)
            {
                choice = IntInputWithValidation();
                switch (choice)
                {
                    case 1:
                        exprLinear = x => x * x - 2;
                        deriLinear = x => 2 * x;
                        exprLinearString = "x^2 - 2";
                        uncorrect = false;
                        break;
                    case 2:
                        exprLinear = x => x * x * x - 2 * x - 5;
                        deriLinear = x => 3 * x * x - 2;
                        exprLinearString = "x^3 - 2x - 5";
                        uncorrect = false;
                        break;
                    case 3:
                        exprLinear = x => Math.Tan(x - 2);
                        deriLinear = x => 1 / (Math.Cos(x - 3)) * (Math.Cos(x - 3));
                        exprLinearString = "tan(x - 3)";
                        uncorrect = false;
                        break;
                    default:
                        continue;
                }
            }


            Console.WriteLine("Enter a range [a, b]:");
            Console.Write("a: ");
            a = IntInputWithValidation();
            Console.Write("b: ");
            b = IntInputWithValidation();

            Console.WriteLine("Specify the stop criterion.");
            Console.WriteLine("1. Accuracy");
            Console.WriteLine("2. Number of iterations");

            choice = IntInputWithValidation();

            if (choice == 1)
            {
                Console.Write("Enter accuracy: ");
                epsilon = DoubleInputWithValidation();
                zeroLinear = FindZeroArgBisection(exprLinear, a, b, epsilon);
                LogResult(exprLinear, exprLinearString, zeroLinear, epsilon, "Bisection");
                zeroLinear = FindZeroArgNewtons(exprLinear, deriLinear, a, b, epsilon);
                LogResult(exprLinear, exprLinearString, zeroLinear, epsilon, "Newton's");
            }
            else
            {
                Console.Write("Enter number of iterations: ");
                iterations = IntInputWithValidation();
                zeroLinear = FindZeroArgBisection(exprLinear, a, b, iterations);
                LogResult(exprLinear, exprLinearString, zeroLinear, iterations, "Bisection");
                zeroLinear = FindZeroArgNewtons(exprLinear, deriLinear, a, b, iterations);
                LogResult(exprLinear, exprLinearString, zeroLinear, iterations, "Newton's");
            }
        }

        private static int IntInputWithValidation()
        {
            bool uncorrect = true;
            int output = 0;
            string input;
            while (uncorrect)
            {
                input = Console.ReadLine();
                if (!int.TryParse(input, out output))
                {
                    Console.WriteLine("Wrong input! Try again.");
                }
                else
                {
                    uncorrect = false;
                }
            }
            return output;
        }

        private static double DoubleInputWithValidation()
        {
            bool uncorrect = true;
            double output = 0;
            string input;
            while (uncorrect)
            {
                input = Console.ReadLine();
                if (!double.TryParse(input, out output))
                {
                    Console.WriteLine("Wrong input! Try again.");
                }
                else
                {
                    uncorrect = false;
                }
            }
            return output;
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

            double x = min;
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
            Console.WriteLine($"Function f(x) = {function} is zero when x = {root}\n(calculated using {method} method, using {iterations} iterations).\nf({root}) = {expression(root):n20}\n");
        }

        #endregion
    }
}
