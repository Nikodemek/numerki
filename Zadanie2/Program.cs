using System.Runtime.InteropServices;
using Zadanie2.Dao;
using Zadanie2.Utils;

namespace Zadanie2;

class Program {
  
    public static void Main()
    {
        Global.EnsureDirectoryIsValid(true);

        var matricesReader = new MatricesReader("matrices.txt");
        var matrices = matricesReader.Read();

        /*foreach (var matrix in matrices)
        {
            matrix.Print();
        }*/

        var test = new double[,]
        {
            {3, -1, 2, -1, -13},
            {3, -1, 1, 1, 1},
            {1, 2, -1, 2, 21},
            {-1, 1, -2, -3, -5},
        };
        
        var test1 = new double[,]
        {
            {0, 0, 1, 3},
            {1, 0, 0, 7},
            {0, 1, 0, 5},
        };

        /*test = GaussSolution.PrepareMatrix(test);
        test.Print();

        test = GaussSolution.Elimination(test);
        test.Print();*/

/*        for (int i = 0; i < test.GetLength(0); i++)
        {
            for (int j = 0; j < test.GetLength(1); j++)
            {
                Console.Write(Math.Round(test[i, j], 4) + " ");
            }
            Console.WriteLine();
        }*/

        var list = GaussSolution.Solve(test1);
        test1.Print();
        list.Reverse();
        foreach (var d in list)
        {
            Console.WriteLine(Math.Round(d, 4));
        }


        Console.ReadKey();
    }
}