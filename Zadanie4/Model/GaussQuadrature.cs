namespace Zadanie4.Model;

public class GaussQuadrature
{
    public double CalculateIntegral(Func<double, double> func, int knotsCount)
    {
        double result = 0;
        double factor = Math.PI / knotsCount;
        for (int i = 0; i < knotsCount; i++)
        {
            double x = Math.Cos((2 * i + 1) / 2 * knotsCount + 2) * Math.PI;
            result += factor * x;
        }

        return result;
    }
}