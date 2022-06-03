namespace Zadanie5.Model;

public readonly record struct Function(
    Func<double, double> Expr,
    string ExprString,
    double RangeMin,
    double RangeMax,
    int Degree);
