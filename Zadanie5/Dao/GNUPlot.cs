using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Zadanie5.Util;

namespace Zadanie5.Dao;

public class GNUPlot : IDisposable
{
    private static readonly string GnuplotPath =
        RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?
        Path.Combine("/", "usr", "bin", "gnuplot") :
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "gnuplot", "bin", "gnuplot.exe");
    private static readonly string BaseDataDirPath = Path.Combine(Path.GetTempPath(), "metody_numeryczne_2022", "zad5");

    private static uint _instances = 0;

    private readonly string DataDirPath;
    private readonly string OrigFunctionDataFilePath;
    private readonly string InterpolationFunctionDataFilePath;

    private StreamWriter? _gpSw;
    private Process? _gpProc;

    public GNUPlot()
    {
        DataDirPath = Path.Combine(BaseDataDirPath, $"assets-({_instances})");
        OrigFunctionDataFilePath = Path.Combine(DataDirPath, "orig_function_data.dat");
        InterpolationFunctionDataFilePath = Path.Combine(DataDirPath, "interpolation_function_data.dat");
        
        Utils.CreateDirectory(DataDirPath);

        _instances++;
    }

    public void FuncDataToFile(Func<double, double> expression, double min, double max, bool funcSwitch, double step = 0.01)
    {
        var stringBuilder = new StringBuilder((int)((max - min) / step));

        for (double x = min; x < max; x += step)
        {
            double y = expression(x);
            stringBuilder.Append(x).Append('\t').Append(y).AppendLine();
        }
        string correctData = stringBuilder.Replace(',', '.').ToString();

        using var writer = new StreamWriter(funcSwitch ? OrigFunctionDataFilePath : InterpolationFunctionDataFilePath);
        writer.WriteLine(correctData);
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

        string ifOrigExist = File.Exists(OrigFunctionDataFilePath)
            ? $"plot '{OrigFunctionDataFilePath}' title \"F(x) - aproksymowany\" w l, "
            : "plot ";

        _gpSw.WriteLine(ifOrigExist +
              $"'{InterpolationFunctionDataFilePath}' title \"f(x) - aproksymacyjny\" w l");
    }

    public void Stop()
    {
        _gpSw?.Dispose();
        _gpSw = null;

        _gpProc?.Dispose();
        _gpProc = null;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        Stop();
        Utils.DeleteDirectory(DataDirPath);
    }
}