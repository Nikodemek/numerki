using Zadanie3.Dao;
using Zadanie3.Model;

namespace Zadanie3;

class Program {

    private static readonly Function[] Functions = {
        new(
            Expr: x => x * x * x - 2 * x - 5,
            ExprString: "x^3 - 2x - 5",
            DefMin: -10.0,
            DefMax: 9.0
            ),
        new(
            Expr: x => Math.Pow(2, x - 2) - 3,
            ExprString: "2^(x - 2) - 1",
            DefMin: -1.0,
            DefMax: 4.5
            ),
        new(
            Expr: x => Math.Sin(x * x - 2),
            ExprString: "sin(x^2 - 2)",
            DefMin: -2.0,
            DefMax: 1.0
            ),
        new(
            Expr: x => Math.Pow(3, Math.Sin(x * x * x - 2)) - 2,
            ExprString: "3^(sin(x^3 - 2)) - 2",
            DefMin: -1.3,
            DefMax: 0.8
            ),
        new(
            Expr: Math.Abs,
            ExprString: "|x|",
            DefMin: -10,
            DefMax: 10
        ),
    };

    public static void Main()
    {
        using var gnuplot = new GNUPlot();

        gnuplot.FuncDataToFile(Functions[4].Expr, -10, 9, true);
        Interpolation interpolation = new Interpolation(Functions[4], -10, 9, 5);

        gnuplot.FuncDataToFile(
            abscissa => interpolation.CalculateValue(abscissa),
            -10, 9, false
            );

        double[,] knots = interpolation.Knots;
        gnuplot.PointDataToFile(knots);
        gnuplot.Start();

        Console.ReadKey();
    }
}