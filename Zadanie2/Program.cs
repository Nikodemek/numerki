using Zadanie2.Dao;
using Zadanie2.Utils;

namespace Zadanie2;

class Program {
  
    public static void Main()
    {
        Global.EnsureDirectoryIsValid(true);

        var matricesReader = new MatricesReader("matrices.txt");
        var matrices = matricesReader.Read();

        foreach (var matrix in matrices)
        {
            matrix.Print();
        }

        var test = new double[,]
        {
            {3, 2, 1, -1},
            {5, -1, 1, 2},
            {1, -1, 1, 2},
            {7, 8, 1, -7},
        };
        test.Print();

        test = GaussSolution.Elimination(test);
        test.Print();

        Console.ReadKey();
    }
}