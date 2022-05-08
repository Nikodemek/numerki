namespace Zadanie3.Utils;

public class ArraysUtil
{
    public static double[,] ConvertToTwoDimensional(List<double[]> doublesList)
    {
        int rowLength = doublesList.Count;
        int columnLength = doublesList[0].Length;

        var newArray = new double[rowLength, columnLength];

        for (var i = 0; i < rowLength; i++)
        {
            var arr = doublesList[i];
            if (arr.Length != columnLength) throw new ArgumentException("List of arrays is not rectangular!", nameof(doublesList));

            for (int j = 0; j < columnLength; j++)
            {
                newArray[i, j] = arr[j];
            }
        }

        return newArray;
    }

    public static (double min, double max) FindMinAndMaxAtColumn(double[,] array, int columnNum)
    {
        int length = array.GetLength(0);
        double value = array[0, columnNum];
        double min = value;
        double max = value;
        for (var i = 1; i < length; i++)
        {
            value = array[i, columnNum];
            if (value > max) max = value;
            else if (value < min) min = value;
        }
        return (min, max);
    }

    public static double CheckDiff(double[,] array, int columnNum)
    {
        int lengthMinOne = array.GetLength(0) - 1;
        double diff = Math.Abs(array[0, columnNum] - array[1, columnNum]);
        for (var i = 1; i < lengthMinOne; i++)
        {
            double nextDiff = Math.Abs(array[i, columnNum] - array[i + 1, columnNum]);
            if (Math.Abs(nextDiff - diff) > 0.00001) throw new ArithmeticException("Differences between subsequent arguments needs to be the same!");
            diff = nextDiff;
        }
        return diff;
    }

    public static (double min, double max) StrechRange(double min, double max, double percent)
    {
        double distance = max - min;
        double addition = distance * percent;
        return (min - addition, max + addition);
    }
}