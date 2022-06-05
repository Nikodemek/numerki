namespace Zadanie5.Model;

using Polynomials;

public class Approximation
{
    private const double TwoOverPI = 2.0 / Math.PI;
    
    private readonly int _degree;
    private readonly int _integralKnots;
    private readonly Func<double, double> _func;
    private readonly IOrthogonalPolynomials _polynomials;
    private readonly double[] _factors;

    public Approximation(Func<double, double> func, int degree, int integralKnots, IOrthogonalPolynomials? polynomials = default)
    {
        _func = func;
        _degree = degree;
        _integralKnots = integralKnots;
        _polynomials = polynomials ?? new ChebyshevsPolynomials();

        _factors = new double[degree + 1];
        for (var i = 0; i < _degree; i++)
        {
            _factors[i] = CalculateFactor(i, _integralKnots);
        }
    }

    public double CalculateError(int steps)
    {
        double error = 0;
        
        double step = 2.0 / steps;
        double position = -1.0;
        while (position <= 1.0)
        {
            double diff = _func.Invoke(position) - CalculateValue(position);
            error += diff * diff;
            position += step;
        }

        return error;
    }

    public double CalculateValue(double x)
    {
        double result = 0.5 * _factors[0];
        for (var i = 1; i <= _degree; i++)
        {
            result += _factors[i] * _polynomials.Get(i, x);
        }

        return result;
    }

    private double CalculateFactor(int index, int knots)
    {
        return TwoOverPI * GaussQuadrature.CalculateIntegral(
            x => _func.Invoke(x) * _polynomials.Get(index, x),
            knots);
    }

}