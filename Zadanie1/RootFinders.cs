using System;

namespace Zadanie1;

public static class RootFinders
{
    public static double FindBisection(Func<double, double> expr, double min, double max, double eps, out int iterations)
    {
        double valueOfMin = expr(min);
        double valueOfMax = expr(max);
        bool isIncreasing = valueOfMin < valueOfMax;

        if (valueOfMin * valueOfMax > 0 || eps <= 0.0) throw new ArgumentException("Złe argumenty");

        double upperBound = isIncreasing ? max : min;
        double lowerBound = isIncreasing ? min : max;

        double prevPotRoot = lowerBound;
        double potRoot = (upperBound + lowerBound) * 0.5;

        iterations = 1;
        while (Math.Abs(prevPotRoot - potRoot) > eps)
        {
            double result = expr(potRoot);

            if (result < 0) lowerBound = potRoot;
            else upperBound = potRoot;

            prevPotRoot = potRoot;
            potRoot = (upperBound + lowerBound) * 0.5;

            iterations++;
        }

        return potRoot;
    }

    public static double FindBisection(Func<double, double> expr, double min, double max, int iters, out double epsilon)
    {
        double valueOfMin = expr(min);
        double valueOfMax = expr(max);
        bool isIncreasing = valueOfMin < valueOfMax;

        if (valueOfMin * valueOfMax > 0 || iters <= 0) throw new ArgumentException("Złe argumenty");

        double upperBound = isIncreasing ? max : min;
        double lowerBound = isIncreasing ? min : max;

        double prevPotRoot = lowerBound;
        double potRoot = (upperBound + lowerBound) * 0.5;

        for (var i = 1; i < iters; i++)
        {
            double result = expr(potRoot);

            if (result < 0) lowerBound = potRoot;
            else upperBound = potRoot;

            prevPotRoot = potRoot;
            potRoot = (upperBound + lowerBound) * 0.5;
        }

        epsilon = Math.Abs(prevPotRoot - potRoot);
        return potRoot;
    }

    public static double FindNewtons(Func<double, double> expr, Func<double, double> deriv, double min, double max, double eps, out int iterations)
    {
        double valueOfMin = expr(min);
        double valueOfMax = expr(max);

        if (valueOfMin * valueOfMax > 0 || eps <= 0.0) throw new ArgumentException("Złe argumenty");

        double prevPotRoot = min;
        double potRoot = prevPotRoot - expr(prevPotRoot) / deriv(prevPotRoot);

        iterations = 1;
        while (Math.Abs(potRoot - prevPotRoot) > eps)
        {
            prevPotRoot = potRoot;
            potRoot = prevPotRoot - expr(prevPotRoot) / deriv(prevPotRoot);

            iterations++;
        }

        return potRoot;
    }

    public static double FindNewtons(Func<double, double> expr, Func<double, double> deriv, double min, double max, int iters, out double epsilon)
    {
        double valueOfMin = expr(min);
        double valueOfMax = expr(max);

        if (valueOfMin * valueOfMax > 0 || iters <= 0) throw new ArgumentException("Złe argumenty");

        double prevPotRoot = min;
        double potRoot = prevPotRoot - expr(prevPotRoot) / deriv(prevPotRoot);

        for (var i = 1; i < iters; i++)
        {
            prevPotRoot = potRoot;
            potRoot = prevPotRoot - expr(prevPotRoot) / deriv(prevPotRoot);
        }

        epsilon = Math.Abs(prevPotRoot - potRoot);
        return potRoot;
    }
}
