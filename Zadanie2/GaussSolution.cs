using Zadanie2.Utils;

namespace Zadanie2;

public class GaussSolution
{
    private static void Elimination(double[,] matrix)
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
    }

    public static List<double> Solve(double[,] matrix)
    {
        var preparedMatrix = Arrayer.Copy(matrix);
        var solutions = new List<double>();
        
        PrepareMatrix(preparedMatrix);
        Elimination(preparedMatrix);

        var columnSize = preparedMatrix.GetLength(0);
        var rowSize = preparedMatrix.GetLength(1);

        var firstSolution = preparedMatrix[columnSize - 1, rowSize - 1] / preparedMatrix[columnSize - 1, columnSize - 1];
        solutions.Add(firstSolution);

        for (var i = columnSize - 2; i >= 0; i--)
        {
            var solution = preparedMatrix[i, rowSize - 1];
            for (var j = 0; j < solutions.Count; j++)
            {
                solution -= solutions[j] * preparedMatrix[i, rowSize - (j + 2)];
            }

            solution /= preparedMatrix[i, i];
            solutions.Add(solution);
        }

        return solutions;
    }

    private static void PrepareMatrix(double[,] matrix)
    {
        var columnSize = matrix.GetLength(0);
        var rowSize = matrix.GetLength(1);
     
        for (var i = 0; i < rowSize; i++)
        {
            for (var j = i; j < columnSize; j++)
            {
                for (var k = j + 1; k < columnSize; k++)
                {
                    if (matrix[j, i] < matrix[k, i])
                    {
                        ChangeRows(matrix, j, k);
                    }
                }
            }
        }
    }

    private static void ChangeRows(double[,] matrix, int a, int b)
    {
        var rowSize = matrix.GetLength(1);
        
        for (var i = 0; i < rowSize; i++)
        {
            (matrix[a, i], matrix[b, i]) = (matrix[b, i], matrix[a, i]);
        }
    }
}