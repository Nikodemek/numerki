namespace Zadanie5.Model;

using Polynomials;

public class Approximation
{
    private readonly IOrthogonalPolynomials _polynomials;

    public Approximation(IOrthogonalPolynomials? polynomials = default)
    {
        _polynomials = polynomials ?? new ChebyshevsPolynomials();
    }

    public double CalculateValue(Func<double, double> func, double x, int degree, int integralKnots)
    {
        double result = 0;
        for (var i = 1; i <= degree; i++)
        {
            result += CalculateFactor(func, i, integralKnots) * _polynomials.Get(i, x);
        }

        result += 0.5d * CalculateFactor(func, 0, integralKnots);

        return result;
    }

    public double CalculateError(int steps, Func<double, double> func, int degree, int integralKnots)
    {
        double error = 0;
        double step = 2.0 / steps;
        double position = -step;
        while (position <= 1.0)
        {
            error += (func.Invoke(position) - CalculateValue(func, position, degree, integralKnots)) * 
                (func.Invoke(position) - CalculateValue(func, position, degree, integralKnots));
            position += step;
        }

        return error;
    }

    private double CalculateFactor(Func<double, double> func, int index, int knots)
    {
        return 2.0 / Math.PI * GaussQuadrature.CalculateIntegral(
            x => func.Invoke(x) * _polynomials.Get(index, x),
            knots);
    }

}