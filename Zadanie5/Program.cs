using System;
using Zadanie5.Model;
using Zadanie5.Util;

namespace Zadanie5;

public class Program
{
    private static readonly Function[] Functions = {
        new(
            Expr: x => 2 * x + 1,
            ExprString: "2x + 1",
            RangeMin: -5.0,
            RangeMax: 5.0,
            Degree: 4
        ),
        new(
            Expr: x => ((x - 0) * x - 2) * x - 5,
            ExprString: "x^3 - 2x - 5",
            RangeMin: -2.5,
            RangeMax: 2.5,
            Degree: 4
        ),
        new(
            Expr: x => Math.Sin(x),
            ExprString: "sin(x)",
            RangeMin: -5.0,
            RangeMax: 5.0,
            Degree: 4
        ),
        new(
            Expr: x => Math.Abs(x),
            ExprString: "|x|",
            RangeMin: -5.0,
            RangeMax: 5.0,
            Degree: 4
        ),
        new(
            Expr: x => Math.Cos(x * x * x + Math.Abs(x) - 2),
            ExprString: "cos(x ^ 3 + |x| - 2)",
            RangeMin: -4.0,
            RangeMax: 3.0,
            Degree: 20
        ),
    };

    public static void Main()
    {
        double x = ChebyshevsPolynomials.Get(3, 2);
        x = ChebyshevsPolynomials.Get(2, 3);
        Console.WriteLine(x);

        int funcNo = 2;
        double rangeMin;
        double rangeMax;
        int degree;

        Console.WriteLine("For which function do you want to calculate the interpolation?");
        int funcsLenght = Functions.Length;
        for (int i = 0; i < funcsLenght; i++)
        {
            Console.WriteLine($"{i + 1}. f(x) = {Functions[i].ExprString}");
        }
        Console.Write($"Function (default = {funcNo}): ");
        funcNo = Utils.ReadInt32(min: 1, max: funcsLenght, def: funcNo) - 1;
        Console.WriteLine();
        
        Function function = Functions[funcNo - 1];

        rangeMin = function.RangeMin;
        Console.Write($"RangeMin (default = {rangeMin}): ");
        rangeMin = Utils.ReadDouble(def: rangeMin);
        
        rangeMax = function.RangeMax;
        Console.Write($"RangeMin (default = {rangeMax}): ");
        rangeMax = Utils.ReadDouble(def: rangeMax);
        
        degree = function.Degree;
        Console.Write($"RangeMin (default = {degree}): ");
        degree = Utils.ReadInt32(def: degree);

        

        Console.ReadLine();
    }
}