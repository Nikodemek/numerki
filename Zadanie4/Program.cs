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
    };
    
    public static void Main(string[] args)
    {

        NewtonCotesQuadrature newtonCotesQuadrature 
            = new NewtonCotesQuadrature(30);

        double result = newtonCotesQuadrature.CalculateIntegralWithBorder(Functions[1].Expr);
        Console.WriteLine(result);

        newtonCotesQuadrature.Accuracy = 20;
        result = newtonCotesQuadrature.CalculateIntegralWithBorder(Functions[1].Expr);
        Console.WriteLine(result);

        newtonCotesQuadrature.Accuracy = 100;
        result = newtonCotesQuadrature.CalculateIntegralWithBorder(Functions[1].Expr);
        Console.WriteLine(result);
    }
}