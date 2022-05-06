using System;

namespace Zadanie1;

internal readonly record struct Function(
    Func<double, double> Expr, 
    Func<double, double> Deriv, 
    string ExprString,
    double DefMin,
    double DefMax
    );
