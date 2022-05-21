namespace Zadanie4.Model;

public class GaussQuadrature
{
    public double CalculateIntegral(Func<double, double> func, int knotsCount)
    {
        double result = 0;
        double factor = Math.PI / (knotsCount + 1);
        for (int i = 0; i < knotsCount; i++)
        {
            double x = Math.Cos((2 * i + 1) * Math.PI / (2 * knotsCount + 2));
            result += factor * func(x);
        }

        return result;
    }
}