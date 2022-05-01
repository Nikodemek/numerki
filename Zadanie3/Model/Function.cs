﻿using System;

namespace Zadanie3.Model;

internal readonly record struct Function(
    Func<double, double> Expr, 
    string ExprString,
    double DefMin,
    double DefMax
    );
