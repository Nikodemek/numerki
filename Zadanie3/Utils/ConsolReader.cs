using System.Globalization;

namespace Zadanie3.Utils;

public static class ConsolReader
{
    public static int ReadInt32(int min = Int32.MinValue, int max = Int32.MaxValue, int def = 0)
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

                if (Int32.TryParse(input, out int output) && output.Between(min, max))
                {
                    return output;
                }
            }

            Console.WriteLine($"Wrong input! It is supposed to be an Int32, ranging from {min} to {max}.");
        }
    }

    public static double ReadDouble(double min = Double.MinValue, double max = Double.MaxValue, double def = 0.0)
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
                    return output;
                }
            }

            Console.WriteLine($"Wrong input! It is supposed to be a Double, ranging from {min} to {max}.");
        }
    }

    public static bool Between(this double val, double min, double max)
    {
        return val >= min && val <= max;
    }

    public static bool Between(this int val, int min, int max)
    {
        return val >= min && val <= max;
    }
}