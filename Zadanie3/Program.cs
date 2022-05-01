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
    };

    public static void Main()
    {
        using var gnuplot = new GNUPlot();

        gnuplot.FuncDataToFile(Functions[0].Expr, Functions[0].DefMin, Functions[0].DefMax, true);
        gnuplot.FuncDataToFile(Functions[1].Expr, Functions[1].DefMin, Functions[1].DefMax, false);
        gnuplot.PointDataToFile(Functions[1].Expr, Functions[1].DefMin, Functions[1].DefMax);

        gnuplot.Start();

        var fileManager = new FileManager("test");
        var sth = fileManager.Read();
        foreach (var d in sth)
        {
            Console.WriteLine(d);
        }

        Console.ReadKey();
    }
}