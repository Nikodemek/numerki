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

        test = GaussSolution.Elimination(test);

        for (int i = 0; i < test.GetLength(0); i++)
        {
            for (int j = 0; j < test.GetLength(1); j++)
            {
                Console.Write(test[i, j] + " ");
            }
            Console.WriteLine();
        }
        
        Console.ReadKey();
    }
}