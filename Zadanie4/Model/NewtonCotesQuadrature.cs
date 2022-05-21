namespace Zadanie4.Model;

public class NewtonCotesQuadrature
{
    public double Accuracy { get; set; }
    public NewtonCotesQuadrature(double accuracy)
    {
        Accuracy = accuracy;
    }

    public double CalculateIntegral(Func<double, double> func, double a, double b)
    {
        if (a > b)
            throw new ArgumentException("Beginning of interval must be a number greater than the end limit");
        
        double comparison;
        double result = 0;
        int div = 2;
        do
        {
            double diff = (b - a) / div;

            double evenElementSum = 0;
            for (var i = 2; i < b; i += 2)
            {
                evenElementSum += func(a + diff * i);
            }

            double oddElementSum = 0;
            for (var i = 1; i < b; i += 2)
            {
                oddElementSum += func(a + diff * i);
            }

            double currentResult = 1.0 / 3.0 * diff * (func(a) + func(b) + 4 * oddElementSum + 2 * evenElementSum);
            comparison = Math.Abs(currentResult - result);
            
            result = currentResult;
            div += 1;
        } while (comparison > Accuracy);

        return result;
    }
}