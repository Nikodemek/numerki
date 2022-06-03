namespace Zadanie5.Model;

public static class ChebyshevsPolynomials
{
    private static readonly List<double> _polynomials = new(8);
    private static double? _lastX = null; 
    
    public static double Get(int k, double x)
    {
        if (x != _lastX) Clear(x);
        _lastX = x;
        
        int count = _polynomials.Count;
        if (count > k) return _polynomials[k];

        double val = 0;

        for (int i = count; i <= k; i++)
        {
            double firstTolast = _polynomials[i - 1];
            double secondToLast = _polynomials[i - 2];

            val = 2 * x * firstTolast - secondToLast;
            _polynomials.Add(val);
        }

        return val;
    }

    private static void Clear(double newX)
    {
        _polynomials.Clear();
        _polynomials.Add(1);
        _polynomials.Add(newX);
    }
}
