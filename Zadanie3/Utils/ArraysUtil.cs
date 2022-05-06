namespace Zadanie3.Utils;

public class ArraysUtil
{
    public static double[,] ConvertToTwoDimensional(List<double[]> doublesList)
    {
        int rowLength = doublesList.Count;
        int columnLength = doublesList[0].Length;

        double[,] newArray = new double[rowLength, columnLength];

        for (var i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < columnLength; j++)
            {
                newArray[i, j] = doublesList[i][j];
            }
        }

        return newArray;
    }

    public static (double min, double max) FindMinAndMaxAtColumn(double[,] array, int columnNum)
    {
        double min = array[0, columnNum];
        double max = array[0, columnNum];
        for (var i = 1; i < array.GetLength(0); i++)
        {
            if (array[i, columnNum] > max)
                max = array[i, columnNum];
            if (array[i, columnNum] < min)
                min = array[i, columnNum];
        }

        return (min, max);
    }
    
    public static double CheckDiff(double[,] array, int columnNum)
    {
        double diff = Math.Abs(array[0, columnNum] - array[1, columnNum]);
        for (var i = 1; i < array.GetLength(0) - 1; i++)
        {
            double nextDiff = Math.Abs(array[i, columnNum] - array[i + 1, columnNum]);
            if (Math.Abs(nextDiff - diff) > 0.00001)
                throw new ArithmeticException("Differences between neighbor arguments needs to be the same!");
            diff = nextDiff;
        }
        
        return diff;
    }
}