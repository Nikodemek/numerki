namespace Zadanie3.Utils;

public class ArrayConverter
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
}