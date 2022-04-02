using System.Text;
using Zadanie2.Dao;
using Zadanie2.Utils;

namespace Zadanie2;
using ESC = GaussSolution.EquationsSystemClass;

class Program {
  
    public static void Main()
    {
        Global.EnsureDirectoryIsValid();

        var matricesReader = new MatricesReader("matrices.txt");
        var matrices = matricesReader.Read();

        /*var test = matrices[1];

        GaussSolution.PrepareMatrix(test);

        test.Print();
        GaussSolution.Elimination(test);*/

        foreach (var mat in matrices[3..4])
        {
            var solutions = GaussSolution.Solve(mat, out var equationsSystemClass);
            var sb = new StringBuilder(solutions.Length * 2);
            sb.Append(Translation(equationsSystemClass));
            sb.Append(", X = { ");
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
            ESC.Independent => "Ukad³ad oznaczony",
            ESC.Dependent => "Ukad³ad nieoznaczony",
            ESC.Inconsistent => "Ukad³ad sprzeczny",
            _ => throw new ArgumentException($"Not recognized EquationSystemClass '{esc}'", nameof(esc))
        };
    }
}