using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Zadanie1;

public class GNUPlot : IDisposable
{
    private static readonly string GnuplotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "gnuplot", "bin", "gnuplot.exe");
    private static readonly string BaseDataDirPath = Path.Combine(Path.GetTempPath(), "metody_numeryczne_2022");

    private static uint instances = 0;

    private readonly string DataDirPath;
    private readonly string FunctionDataFilePath;
    private readonly string PointDataFilePath;

    private StreamWriter _gpSw;
    private Process _gpProc;

    public GNUPlot()
    {
        DataDirPath = Path.Combine(BaseDataDirPath, $"assets-({instances})");
        FunctionDataFilePath = Path.Combine(DataDirPath, "function_data.dat");
        PointDataFilePath = Path.Combine(DataDirPath, "points_data.dat");

        Util.CreateDirectory(DataDirPath);

        instances++;
    }

    public void FuncDataToFile(Func<double, double> expression, double min, double max, double step = 0.1)
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
        writer.Write(correctData);
    }

    public void Start()
    {
        if (_gpProc is not null || _gpSw is not null) Stop();
            
        _gpProc = new Process();
        var startInfo = _gpProc.StartInfo;

        startInfo.FileName = GnuplotPath;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardInput = true;

        _gpProc.Start();

        _gpSw = _gpProc.StandardInput;
        _gpSw.WriteLine($"plot '{FunctionDataFilePath}' title \"f(x)\" w l, " +
            $"'{PointDataFilePath}' every ::0::1 title \"Bisection\" w p, " +
            $"'{PointDataFilePath}' every ::1::2 title \"Newtons\" w p");
    }

    public void Stop()
    {
        if (_gpSw is not null)
        {
            _gpSw.Close();
            _gpSw.Dispose();
            _gpSw = null;
        }        
        if (_gpProc is not null)
        {
            _gpProc.Close();
            _gpProc.Dispose();
            _gpProc = null;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        Stop();
        Util.DeleteDirectory(DataDirPath);
    }
}