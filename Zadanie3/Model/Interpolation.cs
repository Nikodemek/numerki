namespace Zadanie3.Model;

public class Interpolation
{
    private readonly Function _func;
    public double[,] Knots { get; }
    private double _diff;

    public Interpolation(Function func, double min, double max, int knotsCount)
    {
        this._func = func;
        Knots = GetKnots(knotsCount, min, max);
    }

    public Interpolation(double[,] knots)
    {
        Knots = knots;
    }

    private double[,] GetKnots(int knotsCount, double min, double max)
    {
        double[,] knots = new double[knotsCount, 2];
        _diff = (max - min) / (knotsCount - 1);

        for (var i = 0; i < knotsCount; i++)
        {
            knots[i, 0] = min + i * _diff;
            knots[i, 1] = _func.Expr(knots[i, 0]);
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