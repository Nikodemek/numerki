using System.Text;
using Zadanie2.Dao;
using Zadanie2.Utils;

namespace Zadanie2;
using ESC = GaussSolution.EquationsSystemClass;

class Program {
  
    public static void Main()
    {
        Global.EnsureDirectoryIsValid(true);

        var matricesReader = new MatricesReader("matrices.txt");
        var matrices = matricesReader.Read();

        foreach (var mat in matrices)
        {
            var solutions = GaussSolution.Solve(mat, out var equationsSystemClass);

            var sb = new StringBuilder(solutions.Length * 2);
            sb.Append("Uklad ").Append(Translation(equationsSystemClass)).Append(", X = { ");
            foreach (var solution in solutions) sb.Append(solution).Append(", ");
            sb.Remove(sb.Length - 2, 2);
            sb.Append(" }");

            mat.Print();
            Console.WriteLine($"{sb}\n\n\n");
        }

        Console.ReadKey();
    }

    private static string Translation(ESC esc)
    {
        return esc switch
        {
            ESC.Independent => "oznaczony",
            ESC.Dependent => "nieoznaczony",
            ESC.Inconsistent => "sprzeczny",
            _ => throw new ArgumentException($"Not recognized EquationSystemClass '{esc}'", nameof(esc))
        };
    }
}