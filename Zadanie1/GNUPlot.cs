using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Zadanie1;

public class GNUPlot
{
    private static readonly string DataDirPath = @"..\..\..\assets";
    private static readonly string FunctionDataFilePath = Path.Combine(DataDirPath, "function_data.dat");
    private static readonly string PointDataFilePath = Path.Combine(DataDirPath, "point_data.dat");
    private static readonly string GnuplotPath = @"C:\Program Files\gnuplot\bin\gnuplot.exe";

    private StreamWriter _gnupSw;
    private Process _gnupProcess;

    public GNUPlot()
    {
        Util.EnsureDirectoryExists(DataDirPath);
    }

    public void FuncDataToFile(Func<double, double> expression, double min, double max, double step = 0.1d)
    {
        var stringBuilder = new StringBuilder();
        using var writer = new StreamWriter(FunctionDataFilePath);
            
        for (double x = min; x < max; x += step)
        {
            double y = expression(x);
            stringBuilder.Append(x);
            stringBuilder.Append('\t');
            stringBuilder.Append(y);
            stringBuilder.AppendLine();
        }

        string correctData = stringBuilder.ToString().Replace(",", ".");
        writer.WriteLine(correctData);
    }

    public void PointDataToFile(Func<double, double> expression, params double[] xes)
    {
        var stringBuilder = new StringBuilder();
        using var writer = new StreamWriter(PointDataFilePath);

        foreach (var x in xes)
        {
            double y = expression(x);
            stringBuilder.Append(x);
            stringBuilder.Append('\t');
            stringBuilder.Append(y);
            stringBuilder.AppendLine();
        }

        string correctData = stringBuilder.ToString().Replace(",", ".");
        writer.WriteLine(correctData);
    }

    public void Start()
    {
        if (_gnupProcess is not null || _gnupSw is not null) Stop();
            
        _gnupProcess = new Process();
        var startInfo = _gnupProcess.StartInfo;

        startInfo.FileName = GnuplotPath;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardInput = true;

        _gnupProcess.Start();

        _gnupSw = _gnupProcess.StandardInput;
        _gnupSw.WriteLine($"plot '{FunctionDataFilePath}' title \"f(x)\" w l, " +
            $"'{PointDataFilePath}' every ::0::1 title \"Bisection\" w p, " +
            $"'{PointDataFilePath}' every ::1::2 title \"Newtons\" w p");
    }

    public void Stop()
    {
        if (_gnupProcess is null || _gnupSw is null) return;
            
        _gnupSw.Close();
        _gnupSw.Dispose();
        _gnupSw = null;
            
        _gnupProcess.Close();
        _gnupProcess.Dispose();
        _gnupProcess = null;
    }
}