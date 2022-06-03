using Zadanie4.Model;
using Zadanie4.Utils;

namespace Zadanie4;

public class Program
{
    private static readonly Function[] Functions = {
        new(
            Expr: x => ((x - 0) * x - 2) * x - 5,
            ExprString: "x^3 - 2x - 5"
        ),
        new(
            Expr: x => x * x + 3,
            ExprString: "x^2 + 3"
        ),
        new(
            Expr: x => 2 * x + 1,
            ExprString: "2x + 1"
        ),
        new(
            Expr: x => Math.Sin(x),
            ExprString: "sin(x)"
        ),
        new(
            Expr: x => x * x * x,
            ExprString: "x^3"
        ),
    };
    private static readonly Function[] FunctionsPlus = {
        new(
            Expr: x => (((x - 0) * x - 2) * x - 5) / Math.Sqrt(1 - x * x),
            ExprString: "(x^3 - 2x - 5) / sqrt(1 - x^2)"
        ),
        new(
            Expr: x => (x * x + 3) / Math.Sqrt(1 - x * x),
            ExprString: "(x^2 + 3) / sqrt(1 - x^2)"
        ),
        new(
            Expr: x => (2 * x + 1) / Math.Sqrt(1 - x * x),
            ExprString: "(2x + 1) / sqrt(1 - x^2)"
        ),
        new(
            Expr: x => Math.Sin(x) / Math.Sqrt(1 - x * x),
            ExprString: "sin(x) / sqrt(1 - x^2)"
        ),
        new(
            Expr: x => x * x * x / Math.Sqrt(1 - x * x),
            ExprString: "x^3 / sqrt(1 - x^2)"
        ),
    };
    
    public static void Main()
    {
        var rand = new Random();
        int functionsLength = Functions.Length;
        int defFunc = rand.Next(1, functionsLength + 1);

        Console.WriteLine("For which function do you want to calculate the integral?");
        for (int i = 0; i < functionsLength; i++)
        {
            Console.WriteLine($"{i + 1}. f(x) = {Functions[i].ExprString}");
        }
        Console.Write($"Input (default = {defFunc}): ");
        int choice = ConsolReader.ReadInt32(min: 1, max: functionsLength, def: defFunc);
        Console.WriteLine();
        
        var expr = Functions[choice - 1].Expr;
        var exprPlus = FunctionsPlus[choice - 1].Expr;

        double result;
        
        Console.Write("Pass accuracy for Newton-Cotes method (def = 0.01): ");
        double accuracy = ConsolReader.ReadDouble(0, def: 0.01);
        Console.WriteLine();
        result = NewtonCotesQuadrature.CalculateIntegralWithBorder(exprPlus, accuracy);
        Console.WriteLine("Newton-Cotes");
        Console.Write("Indefinite integral in range <-1, 1>: ");
        Console.WriteLine(result);
        const double min = -1.0;
        const double max = 1.0;
        double limitedIntegral = NewtonCotesQuadrature.CalculateIntegral(expr, min, max, accuracy);
        Console.WriteLine($"Newton-Cotes in range <{min}, {max}>: {limitedIntegral}");
        Console.WriteLine();

        Console.WriteLine("Gauss-Chebyshev");
        for (var i = 2; i <= 5; i++)
        {
            result = GaussQuadrature.CalculateIntegral(expr, i);
            Console.Write($"Indefinite integral in range <-1, 1> ({i} knots): ");
            Console.WriteLine(result);
        }

        /*Console.WriteLine("Choose quadrature:");
        Console.WriteLine("1. Newton-Cortes");
        Console.WriteLine("2. Gauss-Chebyshev");
        Console.Write("Input (def = 1): ");
        choice = ConsolReader.ReadInt32(1, 2, 1);
        switch (choice)
        {
            case 1:
                Console.Write("Pass accuracy (def = 0.01): ");
                double accuracy = ConsolReader.ReadDouble(0, def: 0.01);
                NewtonCotesQuadrature newtonCotesQuadrature = new NewtonCotesQuadrature(accuracy);
                result = newtonCotesQuadrature.CalculateIntegralWithBorder(exprPlus);
                Console.Write("Indefinite integral in range <-1, 1>: ");
                Console.WriteLine(result);
                break;
            case 2:
                GaussQuadrature gaussQuadrature = new GaussQuadrature();
                for (var i = 2; i <= 5; i++)
                {
                    result = gaussQuadrature.CalculateIntegral(expr, i);
                    Console.Write($"Indefinite integral in range <-1, 1> ({i} knots): ");
                    Console.WriteLine(result);
                }
                break;
        }*/

        Console.ReadLine();
    }
}