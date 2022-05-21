namespace Zadanie4.Model;

public class NewtonCotesQuadrature
{
    public double Accuracy { get; set; }
    public NewtonCotesQuadrature(double accuracy)
    {
        Accuracy = accuracy;
    }

    private double CalculateIntegral(Func<double, double> func, double a, double b)
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
            for (var i = 2; i < div; i += 2)
            {
                evenElementSum += func(a + diff * i);
            }

            double oddElementSum = 0;
            for (var i = 1; i < div; i += 2)
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

    public double CalculateIntegralWithBorder(Func<double, double> func)
    {
        double result = 0;
        
        double beginning = 0;
        double end = 0.5;
        double integralResult;
       
        do
        {
            integralResult = CalculateIntegral(func, beginning, end);
            result += integralResult;
            beginning = end;
            end += (1 - end) * 0.5;
        } while (Math.Abs(integralResult) > Accuracy);
        
        beginning = -0.5;
        end = 0;
        do
        {
            integralResult = CalculateIntegral(func, beginning, end);
            result += integralResult;
            end = beginning;
            beginning -= (1 - Math.Abs(beginning)) * 0.5;
        } while (Math.Abs(integralResult) > Accuracy);

        return result;
    }
}