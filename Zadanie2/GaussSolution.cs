namespace Zadanie2;

public class GaussSolution
{
    public static double[,] Elimination(double[,] matrix)
    {
        var columnSize = matrix.GetLength(0);
        var rowSize = matrix.GetLength(1);

        for (var i = 0; i < columnSize - 1; i++)
        {
            var pivot = matrix[i, i];

            for (var j = i + 1; j < columnSize; j++)
            {
                var ratio = matrix[j, i] / pivot;
                
                for (var k = 0; k < rowSize; k++)
                {
                    matrix[j, k] -= ratio * matrix[i, k];
                }
            }
        }

        return matrix;
    }

    public static List<double> Solve(double[,] matrix)
    {
        var preparedMatrix = Elimination(matrix);
        var solutions = new List<double>();
        
        var columnSize = preparedMatrix.GetLength(0);
        var rowSize = preparedMatrix.GetLength(1);

        for (var i = 0; i < columnSize; i++)
        {
            for (var j = 0; j < rowSize; j++)
            {
                // TODO
            }
        }

        return solutions;
    }
}