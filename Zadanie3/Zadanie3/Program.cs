using Zadanie3.Dao;

namespace Zadanie3;

class Program {
  
    public static void Main()
    {
        var fileManager = new FileManager("test");
        var sth = fileManager.Read();
        foreach (var d in sth)
        {
            Console.WriteLine(d);
        }

        Console.ReadKey();
    }
}