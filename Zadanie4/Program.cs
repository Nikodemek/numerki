namespace Zadanie4;
using Model;

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
    };
    
    private static readonly Function[] FunctionsPlus = {
        new(
            Expr: x => (2 * x + 1) / Math.Sqrt(1 - x * x),
            ExprString: "2x + 1 / sqrt(1 - x^2)"
        ),
        new(
            Expr: x => Math.Sin(x) / Math.Sqrt(1 - x * x),
            ExprString: "sin(x) / sqrt(1 - x^2)"
        ),
    };
    
    public static void Main(string[] args)
    {

        NewtonCotesQuadrature newtonCotesQuadrature 
            = new NewtonCotesQuadrature(0.01);

        double result = newtonCotesQuadrature.CalculateIntegralWithBorder(FunctionsPlus[1].Expr);
        Console.WriteLine(result);
        
        GaussQuadrature gaussQuadrature = new GaussQuadrature();
        result = gaussQuadrature.CalculateIntegral(Functions[3].Expr, 5);
        Console.WriteLine(result);
    }
}