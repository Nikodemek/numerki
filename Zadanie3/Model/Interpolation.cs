namespace Zadanie3.Model;

public static class Interpolation
{
    public static double[] GetKnots(int knotsCount, double min, double max)
    {
        double[] knots = new double[knotsCount];
        double diff = (max - min) / (knotsCount - 1);
        
        for (var i = 0; i < knotsCount; i++)
            knots[i] = min + i * diff;
        
        return knots;
    }
}