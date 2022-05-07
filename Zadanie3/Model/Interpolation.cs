using Zadanie3.Utils;

namespace Zadanie3.Model;

public class Interpolation
{
    public double[,] Knots { get; init; }

    private readonly double _diff;

    public Interpolation(Func<double, double> func, double min, double max, int knotsCount)
    {
        double diff = CalculateDiff(min, max, knotsCount);
        _diff = diff;
        Knots = CalculateKnots(func, diff, knotsCount, min);
    }

    public Interpolation(double[,] knots)
    {
        _diff = ArraysUtil.CheckDiff(knots, 0);
        Knots = knots;
    }

    /*public double CalculateValue(double abscissa)
    {
        int length = Knots.GetLength(0);

        double t = (abscissa - Knots[0, 0]) / _diff;
        double value = 0;
        
        for (var i = 0; i < length; i++)
        {
            double quotient = Knots[i, 1];
            for (var j = 0; j < length; j++)
            {
                if (i != j) quotient *= (t - j) / (i - j);
            }
            value += quotient;
        }
        
        return value;
    }*/

    public double CalculateValue(double abscissa)
    {
        int length = Knots.GetLength(0);
        double value = 0;
        
        double t = (abscissa - Knots[0, 0]) / _diff;
        for (var i = 0; i < length; i++)
        {
            double quotient = 1;
            for (var j = 0; j < length; j++)
                if (i != j)
                    quotient *= (t - j) / (i - j);

            value += quotient * Knots[i, 1];
        }

        return value;
    }

    private static double CalculateDiff(double min, double max, int knotsCount)
    {
        if (knotsCount < 2) return max - min;
        return (max - min) / (knotsCount - 1);
    }

    private static double[,] CalculateKnots(Func<double, double> func, double diff, int knotsCount, double min)
    {
        var knots = new double[knotsCount, 2];

        double argument = min;
        for (var i = 0; i < knotsCount; i++)
        {
            knots[i, 0] = argument;
            knots[i, 1] = func(argument);
            argument += diff;
        }
        
        return knots;
    }
}