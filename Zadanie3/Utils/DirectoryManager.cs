using System;
using System.Globalization;
using System.IO;

namespace Zadanie3.Utils;

public static class DirectoryManager
{
    public static void CreateDirectory(in string dirPath)
    {
        try
        {
            if (!(dirPath is null || Directory.Exists(dirPath)))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static void DeleteDirectory(in string dirPath)
    {
        try
        {
            Directory.Delete(dirPath, true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}