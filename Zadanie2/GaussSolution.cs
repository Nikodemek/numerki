using Zadanie2.Utils;

namespace Zadanie2;

public class GaussSolution
{
    public static double[] Solve(double[,] matrix, out EquationsSystemClass equationsSystemClass)
    {
        var preparedMatrix = Arrayer.Copy(matrix);

        MakeEchelon(preparedMatrix);
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

    private static void MakeEchelon(double[,] matrix)
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

    private static void GaussianElimination(double[,] matrix)
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

    private static double[] CalculateSolutions(double[,] matrix, int precision = 2)
    {
        int columnSize = matrix.GetLength(0);
        int rowSize = matrix.GetLength(1);

        var solutions = new double[columnSize];

        for (int i = columnSize - 1; i >= 0; i--)
        {
            double solution = matrix[i, rowSize - 1];

            for (var j = 0; j < columnSize - i; j++)
            {
                double variable = solutions[columnSize - j - 1];
                double coefficient = matrix[i, rowSize - j - 2];
                solution -= variable * coefficient;
            }

            solution /= matrix[i, i];
            solutions[i] = Math.Round(solution, precision);
        }
        return solutions;
    }

    private static EquationsSystemClass GetMatrixClass(double[,] matrix)
    {
        int columnSize = matrix.GetLength(0);
        int rowSize = matrix.GetLength(1);

        var allZerosIndices = new List<int>();
        for (var i = 0; i < columnSize; i++)
        {
            bool isAllZeros = true;
            for (var j = 0; j < rowSize - 1; j++)
            {
                if (matrix[i, j] != 0)
                {
                    isAllZeros = false;
                    break;
                }
            }
            if (isAllZeros) allZerosIndices.Add(i);
        }

        foreach (var zeroRows in allZerosIndices)
        {
            if (matrix[zeroRows, rowSize - 1] != 0) return EquationsSystemClass.Inconsistent;
        }

        if (allZerosIndices.Count > 0) return EquationsSystemClass.Dependent;
        else return EquationsSystemClass.Independent;
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