using Zadanie2.Utils;

namespace Zadanie2;

public class GaussSolution
{
    public static double[] Solve(double[,] matrix, out EquationsSystemClass equationsSystemClass)
    {
        var preparedMatrix = Arrayer.Copy(matrix);
        
        MakeTriangular(preparedMatrix);
        GaussianElimination(preparedMatrix);
        equationsSystemClass = GetMatrixClass(preparedMatrix);

        return equationsSystemClass switch
        {
            EquationsSystemClass.Independent => CalculateSolutions(preparedMatrix),
            EquationsSystemClass.Dependent => new double[] { Double.NaN },
            EquationsSystemClass.Inconsistent => new double[] { Double.NaN },
            _ => throw new ArgumentException($"Not recognized EquationSystemClass '{equationsSystemClass}'"),
        };
    }

    public static void MakeTriangular(double[,] matrix)
    {
        int columnSize = matrix.GetLength(0);
        int rowSize = matrix.GetLength(1);

        for (var i = 0; i < rowSize; i++)
        {
            int rowToSwap = i;
            for (var j = i; j < columnSize; j++)
            {
                if (matrix[j, i] > matrix[rowToSwap, i]) rowToSwap = j;
            }
            if (rowToSwap > i) ChangeRows(matrix, i, rowToSwap);
        }
    }

    public static void GaussianElimination(double[,] matrix)
    {
        int columnSize = matrix.GetLength(0);
        int rowSize = matrix.GetLength(1);

        for (var i = 0; i < columnSize - 1; i++)
        {
            double pivot = matrix[i, i];
            for (var j = i + 1; j < columnSize; j++)
            {
                double ratio = matrix[j, i] / pivot;
                matrix[j, i] = 0.0;
                for (var k = i + 1; k < rowSize; k++)
                {
                    double value = matrix[j, k] - ratio * matrix[i, k];
                    matrix[j, k] = (value.IsZero() || !Double.IsFinite(value)) ? 0.0 : value;
                }
            }
        }
    }

    private static double[] CalculateSolutions(double[,] preparedMatrix, int precision = 2)
    {
        int columnSize = preparedMatrix.GetLength(0);
        int rowSize = preparedMatrix.GetLength(1);

        var solutions = new double[columnSize];

        double firstSolution = preparedMatrix[columnSize - 1, rowSize - 1] / preparedMatrix[columnSize - 1, columnSize - 1];
        solutions[0] = Math.Round(firstSolution, precision);

        int counter = 1;
        for (var i = columnSize - 2; i >= 0; i--)
        {
            double solution = preparedMatrix[i, rowSize - 1];
            for (var j = 0; j < counter; j++)
            {
                solution -= solutions[j] * preparedMatrix[i, rowSize - j - 2];
            }

            solution /= preparedMatrix[i, i];
            solutions[counter] = Math.Round(solution, precision);
            counter++;
        }
        return solutions.Reverse();
    }

    private static EquationsSystemClass GetMatrixClass(double[,] matrix)
    {
        int columnSize = matrix.GetLength(0);
        int rowSize = matrix.GetLength(1);
        if (matrix[columnSize - 1, rowSize - 2] == 0.0)
        {
            if (matrix[columnSize - 1, rowSize - 1] == 0.0) return EquationsSystemClass.Dependent;
            return EquationsSystemClass.Inconsistent;
        }
        return EquationsSystemClass.Independent;
    }

    private static void ChangeRows(double[,] matrix, int a, int b)
    {
        int rowSize = matrix.GetLength(1);
        
        for (var i = 0; i < rowSize; i++)
        {
            (matrix[a, i], matrix[b, i]) = (matrix[b, i], matrix[a, i]);
        }
    }


    public enum EquationsSystemClass
    {
        Independent = 1,    // oznaczony
        Dependent = 2,      // nieoznaczony
        Inconsistent = 4,   // sprzeczny
    }
}