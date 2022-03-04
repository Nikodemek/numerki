using System;

namespace Zadanie1;

public class Program
{
    private static GNUPlot _gnuplot = new GNUPlot();
    
    private static void Main()
    {
        MiniMenu();
        _gnuplot.Start();
        
        Console.ReadKey();
        _gnuplot.Stop();
    }

    private static void MiniMenu()
    {
        Func<double, double> expr, deriv;
        string exprString;
        double rangeMin, rangeMax;
        double bisectionRoot, newtonsRoot;
        int choice;

        Console.WriteLine("For which function do you want to calculate the root?");
        Console.WriteLine("1. f(x) = 2x - 3");
        Console.WriteLine("2. f(x) = x^2 - 2");
        Console.WriteLine("3. f(x) = x^3 - 2x - 5");
        Console.WriteLine("4. f(x) = tan(x - 3)");
        Console.WriteLine("5. f(x) = sin(x^2 - 2)");
        Console.WriteLine("6. f(x) = 2^sin(x) - 1");
        Console.Write("Input: ");
        choice = Util.ReadInt32(1, 6);
        Console.WriteLine();

        switch (choice)
        {
            case 1:
                expr = x => 2 * x - 3;
                deriv = _ => 2;
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
        rangeMin = Util.ReadDouble();
        Console.Write("max: ");
        rangeMax = Util.ReadDouble(rangeMin);
        Console.WriteLine();
            
        _gnuplot.FuncDataToFile(expr, rangeMin, rangeMax);

        Console.WriteLine("Specify the stop condition.");
        Console.WriteLine("1. Epsilon");
        Console.WriteLine("2. Iterations");
        Console.Write("Input: ");
        choice = Util.ReadInt32(1, 2);
        Console.WriteLine();

        switch (choice)
        {
            case 1:
                Console.Write("Enter accuracy: ");
                double epsilon = Util.ReadDouble(0);
                Console.WriteLine();

                bisectionRoot = FindRootBisection(expr, rangeMin, rangeMax, epsilon);
                Util.LogResult(expr, exprString, bisectionRoot, epsilon, "Bisection");
                newtonsRoot = FindRootNewtons(expr, deriv, rangeMin, rangeMax, epsilon);
                Util.LogResult(expr, exprString, newtonsRoot, epsilon, "Newton's");

                break;
            case 2:
                Console.Write("Enter number of iterations: ");
                int iterations = Util.ReadInt32(1);
                Console.WriteLine();

                bisectionRoot = FindRootBisection(expr, rangeMin, rangeMax, iterations);
                Util.LogResult(expr, exprString, bisectionRoot, iterations, "Bisection");
                newtonsRoot = FindRootNewtons(expr, deriv, rangeMin, rangeMax, iterations);
                Util.LogResult(expr, exprString, newtonsRoot, iterations, "Newton's");

                break;
            default:
                throw new ArgumentException("That should not have happened.");
        }
        
        _gnuplot.PointDataToFile(expr, bisectionRoot, newtonsRoot);
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
}