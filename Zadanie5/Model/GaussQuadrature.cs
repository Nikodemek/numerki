namespace Zadanie5.Model;

public static class GaussQuadrature
{
    public static double CalculateIntegral(Func<double, double> func, int knotsCount)
    {
        double result = 0;

        double factor = Math.PI / knotsCount;
        double quotient = factor / 2.0;
        for (int i = 1; i <= knotsCount; i++)
        {
            double x = Math.Cos((2 * i - 1) * quotient);
            result += factor * func(x);
        }

        return result;
    }
}