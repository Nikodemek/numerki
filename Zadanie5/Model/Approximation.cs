namespace Zadanie5.Model;

using Polynomials;

public class Approximation
{
    private readonly int _degree;
    private readonly int _integralKnots;
    private readonly Func<double, double> _func;
    private readonly IOrthogonalPolynomials _polynomials;

    private double[] factors;

    public Approximation(Func<double, double> func, int degree, int integralKnots ,IOrthogonalPolynomials? polynomials = default)
    {
        _func = func;
        _degree = degree;
        _integralKnots = integralKnots;
        _polynomials = polynomials ?? new ChebyshevsPolynomials();

        factors = new double[degree + 1];
        for (var i = 0; i < _degree; i++)
        {
            factors[i] = CalculateFactor(i, _integralKnots);
        }
    }

    public double CalculateValue(double x)
    {
        double result = 0.5d * factors[0];
        for (var i = 1; i <= _degree; i++)
        {
            result += factors[i] * _polynomials.Get(i, x);
        }

        return result;
    }

    public double CalculateError(int steps)
    {
        double error = 0;
        double step = 2.0 / steps;
        double position = -step;
        while (position <= 1.0)
        {
            error += (_func.Invoke(position) - CalculateValue(position)) * 
                (_func.Invoke(position) - CalculateValue(position));
            position += step;
        }

        return error;
    }

    private double CalculateFactor(int index, int knots)
    {
        return 2.0 / Math.PI * GaussQuadrature.CalculateIntegral(
            x => _func.Invoke(x) * _polynomials.Get(index, x),
            knots);
    }

}