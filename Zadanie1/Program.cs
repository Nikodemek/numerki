using System;

namespace Zadanie1;

public class Program
{
    private static readonly Function[] Functions = {
        new(
            Expr: x => x * x * x - 2 * x - 5,
            Deriv: x => 3 * x * x - 2,
            ExprString: "x^3 - 2x - 5",
            DefMin: -10.0,
            DefMax: 9.0
            ),
        new(
            Expr: x => Math.Pow(2, x - 2) - 3,
            Deriv: x => Math.Pow(2, x - 2) * Math.Log(2),
            ExprString: "2^(x - 2) - 1",
            DefMin: -1.0,
            DefMax: 4.5
            ),
        new(
            Expr: x => Math.Sin(x * x - 2),
            Deriv: x => 2 * x * Math.Cos(x * x - 2),
            ExprString: "sin(x^2 - 2)",
            DefMin: -2.0,
            DefMax: 1.0
            ),
        new(
            Expr: x => Math.Pow(3, Math.Sin(x * x * x - 2)) - 2,
            Deriv: x => 3 * x * x * Math.Pow(3, Math.Sin(x * x * x - 2)) * Math.Cos(x * x * x - 2) * Math.Log(3),
            ExprString: "3^(sin(x^3 - 2)) - 2",
            DefMin: -1.3,
            DefMax: 0.8
            ),
    };

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
        int funcsLenght = Functions.Length;

        int defFunc = rand.Next(1, funcsLenght + 1);
        int defStopCond = rand.Next(1, 3);
        int defIters = rand.Next(1, 100);
        double defEps = Math.Pow(0.1, rand.Next(1, 10));

        double rangeMin, rangeMax;
        double bisectionRoot, newtonsRoot;
        int choice;

        Console.WriteLine("For which function do you want to calculate the root?");
        for (int i = 0; i < funcsLenght; i++)
        {
            Console.WriteLine($"{i + 1}. f(x) = {Functions[i].ExprString}");
        }
        Console.Write($"Input (default = {defFunc}): ");
        choice = Util.ReadInt32(min: 1, max: funcsLenght, def: defFunc);
        Console.WriteLine();

        (var expr, var deriv, var exprString, var defMin, var defMax) = Functions[choice - 1];

        Console.WriteLine($"Enter a range (min, max):");
        Console.Write($"min (default = {defMin}): ");
        rangeMin = Util.ReadDouble(def: defMin);
        Console.Write($"max (default = {defMax}): ");
        rangeMax = Util.ReadDouble(min: rangeMin, def: defMax);
        Console.WriteLine();


        Console.WriteLine("Specify the stop condition.");
        Console.WriteLine("1. Epsilon");
        Console.WriteLine("2. Iterations");
        Console.Write($"Input (default = {defStopCond}): ");
        choice = Util.ReadInt32(min: 1, max: 2, def: defStopCond);
        Console.WriteLine();

        switch (choice)
        {
            case 1:
                Console.Write($"Enter epsilon (default = {defEps}): ");
                double epsilon = Util.ReadDouble(min: 0, def: defEps);
                Console.WriteLine();

                bisectionRoot = RootFinders.FindBisection(expr, rangeMin, rangeMax, epsilon, out int bisectionIterations);
                Util.LogResult(expr, exprString, bisectionRoot, epsilon, "Bisection", bisectionIterations);
                newtonsRoot = RootFinders.FindNewtons(expr, deriv, rangeMin, rangeMax, epsilon, out int newtonsIterations);
                Util.LogResult(expr, exprString, newtonsRoot, epsilon, "Newton's", newtonsIterations);

                break;
            case 2:
                Console.Write($"Enter number of iterations (default = {defIters}): ");
                int iterations = Util.ReadInt32(min: 1, def: defIters);
                Console.WriteLine();

                bisectionRoot = RootFinders.FindBisection(expr, rangeMin, rangeMax, iterations, out double bisectionEpsilon);
                Util.LogResult(expr, exprString, bisectionRoot, iterations, "Bisection", bisectionEpsilon);
                newtonsRoot = RootFinders.FindNewtons(expr, deriv, rangeMin, rangeMax, iterations, out double newtonsEpsilon);
                Util.LogResult(expr, exprString, newtonsRoot, iterations, "Newton's", newtonsEpsilon);

                break;
            default:
                throw new ArgumentException("That should not have happened.");
        }

        (rangeMin, rangeMax) = Util.FindBestRange(rangeMin, rangeMax, 0.2, bisectionRoot, newtonsRoot);

        gnuplot.FuncDataToFile(expr, rangeMin, rangeMax);
        gnuplot.PointDataToFile(expr, bisectionRoot, newtonsRoot);
    }
}