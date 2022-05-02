namespace Zadanie3.Model;

public class Interpolation
{
    private readonly Function _func;
    private double[,] _knots;
    private double _diff;

    public Interpolation(Function func, double min, double max, int knotsCount)
    {
        this._func = func;
        _knots = GetKnots(knotsCount, min, max);
    }

    public Interpolation(double[,] knots)
    {
        this._knots = knots;
    }

    public double[,] GetKnots(int knotsCount, double min, double max)
    {
        _knots = new double[knotsCount, 2];
        _diff = (max - min) / (knotsCount - 1);

        for (var i = 0; i < knotsCount; i++)
        {
            _knots[i, 0] = min + i * _diff;
            _knots[i, 1] = _func.Expr(_knots[i, 0]);
        }

        return _knots;
    }

    public double CalculateValue(double abscissa)
    {
        double value = 0;
        int length = _knots.GetLength(0);

        double t = (abscissa - _knots[0, 0]) / _diff;
        
        for (var i = 0; i < length; i++)
        {
            double quotient = 1;
            for (var j = 0; j < length; j++)
                if (i != j)
                    quotient *= (t - j) / (i - j);
            
            value += quotient * _knots[i, 1];
        }

        return value;
    }
    
}