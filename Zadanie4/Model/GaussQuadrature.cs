namespace Zadanie4.Model;

public class GaussQuadrature
{
    public double CalculateIntegral(Func<double, double> func, int knotsCount)
    {
        double result = 0;
        
        // to jest z wykładu i jest gorsze XD
        /*double factor = Math.PI / (knotsCount + 1);
        for (int i = 0; i < knotsCount; i++)
        {
            double x = Math.Cos((2 * i + 1) * Math.PI / (2 * knotsCount + 2));
            result += factor * func(x);
        }*/
        
        double factor = Math.PI / knotsCount;
        for (int i = 1; i <= knotsCount; i++)
        {
            double x = Math.Cos((2 * i - 1) * Math.PI / (2 * knotsCount));
            result += factor * func(x);
        }

        return result;
    }
}