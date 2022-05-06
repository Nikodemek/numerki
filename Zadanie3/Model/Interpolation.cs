using Zadanie3.Utils;

namespace Zadanie3.Model;

public class Interpolation
{
    public double[,] Knots { get; }
    private readonly Func<double, double>? _func;
    private double _diff;

    public Interpolation(Func<double, double> func, double min, double max, int knotsCount)
    {
        _func = func;
        Knots = GetKnots(knotsCount, min, max);
    }

    public Interpolation(double[,] knots)
    {
        Knots = knots;
        _diff = ArraysUtil.CheckDiff(Knots, 0);
    }

    private double[,] GetKnots(int knotsCount, double min, double max)
    {
        if (_func is null) return null!;
        
        var knots = new double[knotsCount, 2];
        _diff = (max - min) / (knotsCount - 1);

        for (var i = 0; i < knotsCount; i++)
        {
            knots[i, 0] = min + i * _diff;
            knots[i, 1] = _func(knots[i, 0]);
        }

        return knots;
    }

    public double CalculateValue(double abscissa)
    {
        double value = 0;
        int length = Knots.GetLength(0);

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
}