using System;

namespace Zadanie1;

public class Program
{
    private static void Main()
    {
        using var gnuplot = new GNUPlot();

        MiniMenu(gnuplot);
        gnuplot.Start();
        
        Console.ReadKey();
    }

    private static void MiniMenu(GNUPlot gnuplot)
    {
        var rand = new Random();

        int DefFunc = rand.Next(1, 7);
        int DefStopCond = rand.Next(1, 3);
        int DefIters = rand.Next(1, 500);
        double DefEps = Math.Pow(0.1, rand.Next(1, 10));

        Func<double, double> expr, deriv;
        string exprString;
        double rangeMin, rangeMax;
        double defMin, defMax;
        double bisectionRoot, newtonsRoot;
        int choice;

        Console.WriteLine("For which function do you want to calculate the root?");
        Console.WriteLine("1. f(x) = 2x - 3");
        Console.WriteLine("2. f(x) = x^2 - 2");
        Console.WriteLine("3. f(x) = x^3 - 2x - 5");
        Console.WriteLine("4. f(x) = tan(x - 3)");
        Console.WriteLine("5. f(x) = sin(x^2 - 2)");
        Console.WriteLine("6. f(x) = 2^sin(x) - 1");
        Console.Write($"Input (default = {DefFunc}): ");
        choice = Util.ReadInt32(min: 1, max: 6, def: DefFunc);
        Console.WriteLine();

        switch (choice)
        {
            case 1:
                expr = x => 2 * x - 3;
                deriv = _ => 2;
                exprString = "2x - 3";
                defMin = -2.0;
                defMax = 5.0;
                break;
            case 2:
                expr = x => x * x - 2;
                deriv = x => 2 * x;
                exprString = "x^2 - 2";
                defMin = -1.0;
                defMax = 4.0;
                break;
            case 3:
                expr = x => x * x * x - 2 * x - 5;
                deriv = x => 3 * x * x - 2;
                exprString = "x^3 - 2x - 5";
                defMin = -10.0;
                defMax = 9.0;
                break;
            case 4:
                expr = x => Math.Tan(x - 3);
                deriv = x => {
                    double cos = Math.Cos(x - 3);
                    return 1 / (cos * cos);
                };
                exprString = "tan(x - 3)";
                defMin = 1.0;
                defMax = 4.0;
                break;
            case 5:
                expr = x => Math.Sin(x * x - 2);
                deriv = x => 2 * x * Math.Cos(x * x - 2);
                exprString = "sin(x^2 - 2)";
                defMin = -2.0;
                defMax = 1.0;
                break;
            case 6:
                expr = x => Math.Pow(2, Math.Sin(x)) - 1;
                deriv = x => Math.Pow(2, Math.Sin(x)) * Math.Cos(x) * Math.Log(2);
                exprString = "2^sin(x) - 1";
                defMin = 0.1;
                defMax = 3.2;
                break;
            default:
                throw new ArgumentException("That should not have happened.");
        }

        Console.WriteLine($"Enter a range [min, max]:");
        Console.Write($"min (default = {defMin}): ");
        rangeMin = Util.ReadDouble(def: defMin);
        Console.Write($"max (default = {defMax}): ");
        rangeMax = Util.ReadDouble(min: rangeMin, def: defMax);
        Console.WriteLine();

        gnuplot.FuncDataToFile(expr, rangeMin, rangeMax);

        Console.WriteLine("Specify the stop condition.");
        Console.WriteLine("1. Epsilon");
        Console.WriteLine("2. Iterations");
        Console.Write($"Input (default = {DefStopCond}): ");
        choice = Util.ReadInt32(min: 1, max: 2, def: DefStopCond);
        Console.WriteLine();

        switch (choice)
        {
            case 1:
                Console.Write($"Enter epsilon (default = {DefEps}): ");
                double epsilon = Util.ReadDouble(min: 0, def: DefEps);
                Console.WriteLine();

                bisectionRoot = FindRootBisection(expr, rangeMin, rangeMax, epsilon, out int bisectionIterations);
                Util.LogResult(expr, exprString, bisectionRoot, epsilon, "Bisection", bisectionIterations);
                newtonsRoot = FindRootNewtons(expr, deriv, rangeMin, rangeMax, epsilon, out int newtonsIterations);
                Util.LogResult(expr, exprString, newtonsRoot, epsilon, "Newton's", newtonsIterations);

                break;
            case 2:
                Console.Write($"Enter number of iterations (default = {DefIters}): ");
                int iterations = Util.ReadInt32(min: 1, def: DefIters);
                Console.WriteLine();

                bisectionRoot = FindRootBisection(expr, rangeMin, rangeMax, iterations, out double bisectionEpsilon);
                Util.LogResult(expr, exprString, bisectionRoot, iterations, "Bisection", bisectionEpsilon);
                newtonsRoot = FindRootNewtons(expr, deriv, rangeMin, rangeMax, iterations, out double newtonsEpsilon);
                Util.LogResult(expr, exprString, newtonsRoot, iterations, "Newton's", newtonsEpsilon);

                break;
            default:
                throw new ArgumentException("That should not have happened.");
        }

        gnuplot.PointDataToFile(expr, bisectionRoot, newtonsRoot);
    }

    #region Bisection method

    private static double FindRootBisection(Func<double, double> expr, double min, double max, double eps, out int iterations)
    {
        double valueOfMin = expr(min);
        double valueOfMax = expr(max);
        bool isIncreasing = valueOfMin < valueOfMax;

        if (valueOfMin * valueOfMax > 0 || eps <= 0.0) throw new ArgumentException("Złe argumenty");

        double upperBound = isIncreasing ? max : min;
        double lowerBound = isIncreasing ? min : max;

        double prevPotRoot = lowerBound;
        double potRoot = (upperBound + lowerBound) * 0.5;

        iterations = 1;
        while (Math.Abs(prevPotRoot - potRoot) > eps)
        {
            double result = expr(potRoot);

            if (result < 0) lowerBound = potRoot;
            else upperBound = potRoot;

            prevPotRoot = potRoot;
            potRoot = (upperBound + lowerBound) * 0.5;

            iterations++;
        }

        return potRoot;
    }

    private static double FindRootBisection(Func<double, double> expr, double min, double max, int iters, out double epsilon)
    {
        double valueOfMin = expr(min);
        double valueOfMax = expr(max);
        bool isIncreasing = valueOfMin < valueOfMax;

        if (valueOfMin * valueOfMax > 0 || iters <= 0) throw new ArgumentException("Złe argumenty");

        double upperBound = isIncreasing ? max : min;
        double lowerBound = isIncreasing ? min : max;

        double prevPotRoot = lowerBound;
        double potRoot = (upperBound + lowerBound) * 0.5;

        for (var i = 1; i < iters; i++)
        {
            double result = expr(potRoot);

            if (result < 0) lowerBound = potRoot;
            else upperBound = potRoot;

            prevPotRoot = potRoot;
            potRoot = (upperBound + lowerBound) * 0.5;
        }

        epsilon = Math.Abs(prevPotRoot - potRoot);
        return potRoot;
    }

    #endregion

    #region Newton's method

    private static double FindRootNewtons(Func<double, double> expr, Func<double, double> deriv, double min, double max, double eps, out int iterations)
    {
        double valueOfMin = expr(min);
        double valueOfMax = expr(max);

        if (valueOfMin * valueOfMax > 0 || eps <= 0.0) throw new ArgumentException("Złe argumenty");

        double prevPotRoot = (max + min) * 0.5;
        double potRoot = prevPotRoot - expr(prevPotRoot) / deriv(prevPotRoot);

        iterations = 1;
        while (Math.Abs(potRoot - prevPotRoot) > eps)
        {
            prevPotRoot = potRoot;
            potRoot = prevPotRoot - expr(prevPotRoot) / deriv(prevPotRoot);

            iterations++;
        }

        return potRoot;
    }

    private static double FindRootNewtons(Func<double, double> expr, Func<double, double> deriv, double min, double max, int iters, out double epsilon)
    {
        double valueOfMin = expr(min);
        double valueOfMax = expr(max);

        if (valueOfMin * valueOfMax > 0 || iters <= 0) throw new ArgumentException("Złe argumenty");

        double prevPotRoot = (max + min) * 0.5;
        double potRoot = prevPotRoot - expr(prevPotRoot) / deriv(prevPotRoot);

        for (var i = 1; i < iters; i++)
        {
            prevPotRoot = potRoot;
            potRoot = prevPotRoot - expr(prevPotRoot) / deriv(prevPotRoot);
        }

        epsilon = Math.Abs(prevPotRoot - potRoot);
        return potRoot;
    }

    #endregion
}