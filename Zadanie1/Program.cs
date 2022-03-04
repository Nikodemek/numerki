using System;
using System.Globalization;
using System.IO;

/*                           ''        
    '||''|, .|''|, `||''|,   ||  ('''' 
     ||  || ||..||  ||  ||   ||   `'') 
     ||..|' `|...  .||  ||. .||. `...' 
     ||                                
    .|| 
*/

namespace Zadanie1
{
    static class Program
    {
        private static void Main()
        {
            CheckIfDirectoryExists("../../../assets");

            //QuickCheck();
            MiniMenu();
            using var process = GNUPlot.Run();

            Console.ReadKey();
        }

        private static void QuickCheck()
        {
            Func<double, double> expr;
            Func<double, double> deriv;
            string exprString = "x^2 - 2";
            double epsilon = 0.000001;
            int iterations = 7;

            expr = x => Math.Pow(2, Math.Sin(x)) - 1;
            deriv = x => Math.Pow(2, Math.Sin(x)) * Math.Cos(x) * Math.Log(2);
            exprString = "2^sin(x) - 1";

            int min = -2;
            int max = 1;

            double root;
            root = FindRootBisection(expr, min, max, epsilon);
            LogResult(expr, exprString, root, epsilon, "Bisection");
            root = FindRootNewtons(expr, deriv, min, max, epsilon);
            LogResult(expr, exprString, root, epsilon, "Newton's");

            root = FindRootBisection(expr, min, max, iterations);
            LogResult(expr, exprString, root, iterations, "Bisection");
            root = FindRootNewtons(expr, deriv, min, max, iterations);
            LogResult(expr, exprString, root, iterations, "Newton's");
        }

        private static void MiniMenu()
        {
            Func<double, double> expr, deriv;
            string exprString;
            double rangeMin, rangeMax;
            double root;
            int choice;

            Console.WriteLine("For which function do you want to calculate the root?");
            Console.WriteLine("1. f(x) = 2x - 3");
            Console.WriteLine("2. f(x) = x^2 - 2");
            Console.WriteLine("3. f(x) = x^3 - 2x - 5");
            Console.WriteLine("4. f(x) = tan(x - 3)");
            Console.WriteLine("5. f(x) = sin(x^2 - 2)");
            Console.WriteLine("6. f(x) = 2^sin(x) - 1");
            Console.Write("Input: ");
            choice = ReadInt32(1, 6);
            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    expr = x => 2 * x - 3;
                    deriv = x => 2;
                    exprString = "2x - 3";
                    break;
                case 2:
                    expr = x => x * x - 2;
                    deriv = x => 2 * x;
                    exprString = "x^2 - 2";
                    break;
                case 3:
                    expr = x => x * x * x - 2 * x - 5;
                    deriv = x => 3 * x * x - 2;
                    exprString = "x^3 - 2x - 5";
                    break;
                case 4:
                    expr = x => Math.Tan(x - 3);
                    deriv = x => {
                        double cos = Math.Cos(x - 3);
                        return 1 / (cos * cos);
                    };
                    exprString = "tan(x - 3)";
                    break;
                case 5:
                    expr = x => Math.Sin(x * x - 2);
                    deriv = x => 2 * x * Math.Cos(x * x - 2);
                    exprString = "sin(x^2 - 2)";
                    break;
                case 6:
                    expr = x => Math.Pow(2, Math.Sin(x)) - 1;
                    deriv = x => Math.Pow(2, Math.Sin(x)) * Math.Cos(x) * Math.Log(2);
                    exprString = "2^sin(x) - 1";
                    break;
                default:
                    throw new ArgumentException("That should not have happened.");
            }

            Console.WriteLine("Enter a range [min, max]:");
            Console.Write("min: ");
            rangeMin = ReadDouble();
            Console.Write("max: ");
            rangeMax = ReadDouble(rangeMin);
            Console.WriteLine();
            
            GNUPlot.FuncDataToFile(expr, rangeMin, rangeMax);

            Console.WriteLine("Specify the stop condition.");
            Console.WriteLine("1. Epsilon");
            Console.WriteLine("2. Iterations");
            Console.Write("Input: ");
            choice = ReadInt32(1, 2);
            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    Console.Write("Enter accuracy: ");
                    double epsilon = ReadDouble(0);
                    Console.WriteLine();

                    root = FindRootBisection(expr, rangeMin, rangeMax, epsilon);
                    LogResult(expr, exprString, root, epsilon, "Bisection");
                    root = FindRootNewtons(expr, deriv, rangeMin, rangeMax, epsilon);
                    LogResult(expr, exprString, root, epsilon, "Newton's");

                    break;
                case 2:
                    Console.Write("Enter number of iterations: ");
                    int iterations = ReadInt32(1);
                    Console.WriteLine();

                    root = FindRootBisection(expr, rangeMin, rangeMax, iterations);
                    LogResult(expr, exprString, root, iterations, "Bisection");
                    root = FindRootNewtons(expr, deriv, rangeMin, rangeMax, iterations);
                    LogResult(expr, exprString, root, iterations, "Newton's");

                    break;
                default:
                    throw new ArgumentException("That should not have happened.");
            }
        }

        #region Bisection method

        private static double FindRootBisection(Func<double, double> expr, double min, double max, double eps)
        {
            double valueOfMin = expr(min);
            double valueOfMax = expr(max);
            bool isIncreasing = valueOfMin < valueOfMax;

            if (valueOfMin * valueOfMax > 0 || eps <= 0.0) throw new ArgumentException("Złe argumenty");

            double upperBound = isIncreasing ? max : min;
            double lowerBound = isIncreasing ? min : max;

            double prevPotRoot = lowerBound;
            double potRoot = (upperBound + lowerBound) * 0.5;

            while (Math.Abs(prevPotRoot - potRoot) > eps)
            {
                double result = expr(potRoot);

                if (result < 0) lowerBound = potRoot;
                else upperBound = potRoot;

                prevPotRoot = potRoot;
                potRoot = (upperBound + lowerBound) * 0.5;
            }

            return potRoot;
        }

        private static double FindRootBisection(Func<double, double> expr, double min, double max, int iterations)
        {
            double valueOfMin = expr(min);
            double valueOfMax = expr(max);
            bool isIncreasing = valueOfMin < valueOfMax;

            if (valueOfMin * valueOfMax > 0 || iterations <= 0) throw new ArgumentException("Złe argumenty");

            double upperBound = isIncreasing ? max : min;
            double lowerBound = isIncreasing ? min : max;

            double potRoot = (upperBound + lowerBound) * 0.5;

            for (var i = 0; i < iterations - 1; i++)
            {
                double result = expr(potRoot);

                if (result < 0) lowerBound = potRoot;
                else upperBound = potRoot;

                potRoot = (upperBound + lowerBound) * 0.5;
            }

            return potRoot;
        }

        #endregion

        #region Newton's method

        private static double FindRootNewtons(Func<double, double> expr, Func<double, double> deriv, double min, double max, double eps)
        {
            double valueOfMin = expr(min);
            double valueOfMax = expr(max);
            bool isIncreasing = valueOfMin < valueOfMax;

            if (valueOfMin * valueOfMax > 0 || eps <= 0.0) throw new ArgumentException("Złe argumenty");

            double prevPotRoot = isIncreasing ? max : min;
            double potRoot = prevPotRoot - expr(prevPotRoot) / deriv(prevPotRoot);

            while (Math.Abs(potRoot - prevPotRoot) > eps)
            {
                prevPotRoot = potRoot;
                potRoot = prevPotRoot - expr(prevPotRoot) / deriv(prevPotRoot);
            }

            return potRoot;
        }

        private static double FindRootNewtons(Func<double, double> expr, Func<double, double> deriv, double min, double max, int iterations)
        {
            double valueOfMin = expr(min);
            double valueOfMax = expr(max);
            bool isIncreasing = valueOfMin < valueOfMax;

            if (valueOfMin * valueOfMax > 0 || iterations <= 0) throw new ArgumentException("Złe argumenty");

            double prevPotRoot = isIncreasing ? max : min;
            double potRoot = prevPotRoot - expr(prevPotRoot) / deriv(prevPotRoot);

            for (var i = 0; i < iterations - 1; i++)
            {
                prevPotRoot = potRoot;
                potRoot = prevPotRoot - expr(prevPotRoot) / deriv(prevPotRoot);
            }

            return potRoot;
        }

        #endregion

        #region Utils

        private static int ReadInt32(int min = Int32.MinValue, int max = Int32.MaxValue)
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (Int32.TryParse(input, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out int output) && output.Between(min, max))
                {
                    return output;
                }
                else
                {
                    Console.WriteLine($"Wrong input! It is supposed to be an Int32, ranging from {min} to {max}.");
                }
            }
        }

        private static double ReadDouble(double min = Double.MinValue, double max = Double.MaxValue)
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (Double.TryParse(input, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double output) && output.Between(min, max))
                {
                    return output;
                }
                else
                {
                    Console.WriteLine($"Wrong input! It is supposed to be a Double, ranging from {min} to {max}.");
                }
            }
        }

        private static void LogResult(Func<double, double> expression, string function, double root, double epsilon, string method)
        {
            Console.WriteLine($"Function f(x) = {function} is zero when x = {root:n20}\n(calculated using {method} method, with a precision of {epsilon}).\nf({root:n20}) = {expression(root):n20}\n");
        }

        private static void LogResult(Func<double, double> expression, string function, double root, int iterations, string method)
        {
            Console.WriteLine($"Function f(x) = {function} is zero when x = {root:n20}\n(calculated using {method} method, after {iterations} iterations).\nf({root:n20}) = {expression(root):n20}\n");
        }

        private static bool Between(this double val, double min, double max)
        {
            return val >= min && val <= max;
        }

        private static bool Between(this int val, int min, int max)
        {
            return val >= min && val <= max;
        }

        private static void CheckIfDirectoryExists(string dirPath)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion
    }
}
