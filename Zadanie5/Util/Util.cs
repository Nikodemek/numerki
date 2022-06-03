using System;
using System.Globalization;
using System.IO;

namespace Zadanie5.Util;

public static class Utils
{
    public static int ReadInt32(int min = Int32.MinValue, int max = Int32.MaxValue, int def = 0, Predicate<int>? predicate = default)
    {
        if (!def.Between(min, max)) def = min;

        while (true)
        {
            string? input = Console.ReadLine();

            if (input is not null)
            {
                if (String.Equals(input, String.Empty, StringComparison.OrdinalIgnoreCase))
                {
                    return def;
                }

                if (Int32.TryParse(input, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out int output) && output.Between(min, max))
                {
                    if (predicate?.Invoke(output) is bool valid)
                    {
                        if (valid) return output;
                    }
                    else return output;
                }
            }

            Console.WriteLine($"Wrong input! It is supposed to be an Int32, ranging from {min} to {max}.");
        }
    }
    
    public static double ReadDouble(double min = Double.MinValue, double max = Double.MaxValue, double def = 0.0, Predicate<double>? predicate = default)
    {
        while (true)
        {
            string? input = Console.ReadLine();

            if (input is not null)
            {
                if (String.Equals(input, String.Empty, StringComparison.OrdinalIgnoreCase))
                {
                    return def;
                }

                if (Double.TryParse(input, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double output) && output.Between(min, max))
                {
                    if (predicate?.Invoke(output) is bool valid)
                    {
                        if (valid) return output;
                    }
                    else return output;
                }
            }

            Console.WriteLine($"Wrong input! It is supposed to be a Double, ranging from {min} to {max}.");
        }
    }

    public static void LogResult(Func<double, double> expression, string function, double root, double epsilon, string method, int neededIterations)
    {
        Console.WriteLine($"Function f(x) = {function} is zero when x = {root:n20}\n" +
            $"(calculated using {method} method, with a precision of {epsilon}).\n" +
            $"f({root:n20}) = {expression(root):n20}\n" +
            $"Needed iterations = {neededIterations}\n");
    }

    public static void LogResult(Func<double, double> expression, string function, double root, int iterations, string method, double achievedEpsilon)
    {
        Console.WriteLine($"Function f(x) = {function} is zero when x = {root:n20}\n" +
            $"(calculated using {method} method, after {iterations} iterations).\n" +
            $"f({root:n20}) = {expression(root):n20}\n" +
            $"Achieved accuracy = {achievedEpsilon}\n");
    }

    public static bool Between(this double val, double min, double max)
    {
        return val >= min && val <= max;
    }

    public static bool Between(this int val, int min, int max)
    {
        return val >= min && val <= max;
    }

    public static (double min, double max) FindBestRange(double min, double max, double extend, params double[] points)
    {
        double length = max - min;
        extend *= length;

        foreach (var point in points)
        {
            bool isInside = point.Between(min, max);
            if (!isInside)
            {
                if (point < min)
                {
                    min = point - extend;
                }
                else
                {
                    max = point + extend;
                }
            }
        }

        return (min, max);
    }

    public static void CreateDirectory(in string dirPath)
    {
        try
        {
            if (Directory.Exists(dirPath)) return;
            
            Directory.CreateDirectory(dirPath);
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