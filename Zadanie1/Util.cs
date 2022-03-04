using System;
using System.Globalization;
using System.IO;

namespace Zadanie1;

public static class Util
{
    public static int ReadInt32(int min = Int32.MinValue, int max = Int32.MaxValue)
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (Int32.TryParse(input, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out int output) && output.Between(min, max))
            {
                return output;
            }

            Console.WriteLine($"Wrong input! It is supposed to be an Int32, ranging from {min} to {max}.");
        }
    }

    public static double ReadDouble(double min = Double.MinValue, double max = Double.MaxValue)
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (Double.TryParse(input, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double output) && output.Between(min, max))
            {
                return output;
            }

            Console.WriteLine($"Wrong input! It is supposed to be a Double, ranging from {min} to {max}.");
        }
    }

    public static void LogResult(Func<double, double> expression, string function, double root, double epsilon, string method)
    {
        Console.WriteLine($"Function f(x) = {function} is zero when x = {root:n20}\n(calculated using {method} method, with a precision of {epsilon}).\nf({root:n20}) = {expression(root):n20}\n");
    }

    public static void LogResult(Func<double, double> expression, string function, double root, int iterations, string method)
    {
        Console.WriteLine($"Function f(x) = {function} is zero when x = {root:n20}\n(calculated using {method} method, after {iterations} iterations).\nf({root:n20}) = {expression(root):n20}\n");
    }

    public static bool Between(this double val, double min, double max)
    {
        return val >= min && val <= max;
    }

    public static bool Between(this int val, int min, int max)
    {
        return val >= min && val <= max;
    }

    public static void CreateDirectory(in string dirPath)
    {
        try
        {
            if (dirPath != null && !Directory.Exists(dirPath))
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