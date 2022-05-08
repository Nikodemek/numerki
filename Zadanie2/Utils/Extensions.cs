using System.Text;

namespace Zadanie2.Utils;

public static class Extensions
{
    public static void Print(this double[,] matrix, int digits = 10)
    {
        if (matrix is null) throw new ArgumentException("Matrix can not be null");

        int xLength = matrix.GetLength(0);

        if (xLength <= 0) throw new ArgumentException("Matrix can not be empty");

        int yLength = matrix.GetLength(1);

        string[,] values = new string[xLength, yLength];

        int maxValLen = 0;
        for (int i = 0; i < xLength; i++)
        {
            for (int j = 0; j < yLength; j++)
            {
                double value = matrix[i, j];
                string valueToInsert = value.ToString($"g{digits}");
                int valueToInsertLen = valueToInsert.Length;

                values[i, j] = valueToInsert;
                if (valueToInsertLen > maxValLen) maxValLen = valueToInsertLen;
            }
        }

        var sb = new StringBuilder();

        for (int i = 0; i < xLength; i++)
        {
            sb.Append('[');
            for (int j = 0; j < yLength; j++)
            {
                string valueToInsert = values[i, j].PadLeft(maxValLen, ' ');
                sb.Append(' ').Append(valueToInsert).Append(' ');
            }
            sb.Append("]\n");
        }

        Console.WriteLine(sb.ToString());
    }

    public static bool IsZero(this double d)
    {
        double tolerance = Global.DoubleTolerance;
        return d <= tolerance && d >= -tolerance;
    }
}
