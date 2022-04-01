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

        Console.ReadKey();
    }
}