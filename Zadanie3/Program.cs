using System.Text;
using Zadanie3.Dao;
using Zadanie3.Model;
using Zadanie3.Utils;

namespace Zadanie3;

class Program
{

    private static readonly Function[] Functions = {
        new(
            Expr: x => x * x * x - 2 * x - 5,
            ExprString: "x^3 - 2x - 5"
        ),
        new(
            Expr: x => Math.Pow(2, x - 2) - 3,
            ExprString: "2^(x - 2) - 1"
        ),
        new(
            Expr: x => Math.Sin(x * x - 2),
            ExprString: "sin(x^2 - 2)"
        ),
        new(
            Expr: x => Math.Pow(3, Math.Sin(x * x * x - 2)) - 2,
            ExprString: "3^(sin(x^3 - 2)) - 2"
        ),
        new(
            Expr: x => Math.Abs(x),
            ExprString: "|x|"
        ),
    };

    public static void Main()
    {
        CreateDefaultFile();

        using var gnuplot = new GNUPlot();
        MiniMenu(gnuplot);
        gnuplot.Start();

        Console.ReadKey();
        gnuplot.Stop();
    }

    private static void MiniMenu(GNUPlot gnuplot)
    {
        Interpolation interpolation;
        double rangeMin, rangeMax;
        FileManager fileManager;
        double[,] knots;

        Console.WriteLine("What would you want to do?");
        Console.WriteLine("1. Choose between given function.");
        Console.WriteLine("2. Insert your own knots.");
        Console.Write("Input (def = 1): ");
        int choice = ConsolReader.ReadInt32(1, 2, 1);
        switch (choice)
        {
            case 1:
                var rand = new Random();
                int funcsLenght = Functions.Length;

                int defFunc = rand.Next(1, funcsLenght + 1);
                double defMin = -10.0d;
                double defMax = 10.0d;

                int defKnots = 10;

                Console.WriteLine("For which function do you want to calculate the interpolation?");
                for (int i = 0; i < funcsLenght; i++)
                {
                    Console.WriteLine($"{i + 1}. f(x) = {Functions[i].ExprString}");
                }
                Console.Write($"Input (default = {defFunc}): ");
                choice = ConsolReader.ReadInt32(min: 1, max: funcsLenght, def: defFunc);
                Console.WriteLine();

                var (expr, _) = Functions[choice - 1];

                Console.WriteLine($"Enter a range (min, max):");
                Console.Write($"min (default = {defMin}): ");
                rangeMin = ConsolReader.ReadDouble(def: defMin);
                Console.Write($"max (default = {defMax}): ");
                rangeMax = ConsolReader.ReadDouble(min: rangeMin, def: defMax);
                Console.WriteLine();

                Console.WriteLine($"Enter knots number:");
                Console.Write($"Input (default = {defKnots}): ");
                var knotsCount = ConsolReader.ReadInt32(0, 100, defKnots);
                Console.WriteLine();

                interpolation = new Interpolation(expr, rangeMin, rangeMax, knotsCount);

                gnuplot.FuncDataToFile(expr, rangeMin, rangeMax, true);
                break;

            case 2:
                Console.Write("Pass the filename (def = 'default'): ");
                string filename = Console.ReadLine() ?? String.Empty;
                if (String.IsNullOrEmpty(filename)) filename = "default";
                fileManager = new FileManager(filename);

                knots = fileManager.Read();
                (rangeMin, rangeMax) = ArraysUtil.FindMinAndMaxAtColumn(knots, 0);
                (rangeMin, rangeMax) = ArraysUtil.StrechRange(rangeMin, rangeMax, 0.3);
                interpolation = new Interpolation(knots);
                break;

            default:
                fileManager = new FileManager("default");

                knots = fileManager.Read();
                (rangeMin, rangeMax) = ArraysUtil.FindMinAndMaxAtColumn(knots, 0);
                interpolation = new Interpolation(knots);
                break;
        }
        gnuplot.FuncDataToFile(interpolation.CalculateValue, rangeMin, rangeMax, false);
        gnuplot.PointDataToFile(interpolation.Knots);
    }

    private static void CreateDefaultFile()
    {
        Global.EnsureDirectoryIsValid();
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("1 2");
        stringBuilder.AppendLine("2 3");
        stringBuilder.AppendLine("3 2");
        stringBuilder.AppendLine("4 1");

        using var streamWriter = new StreamWriter(Path.Combine(Global.BaseDataDirPath, "default"));
        streamWriter.Write(stringBuilder);
    }
}