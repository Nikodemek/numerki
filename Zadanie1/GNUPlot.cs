using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Zadanie1;

public class GNUPlot
{
    private static readonly string dataDirPath = @"..\..\..\assets";
    private static readonly string dataFilePath = Path.Combine(dataDirPath, "data.dat");
    private static readonly string gnuplotPath = @"C:\Program Files\gnuplot\bin\gnuplot.exe";

    private StreamWriter gnupSw;
    private Process gnupProcess;

    public GNUPlot()
    {
        Util.EnsureDirectoryExists(dataDirPath);
    }

    public void FuncDataToFile(Func<double, double> expression, double min, double max, double step = 0.1d)
    {
        var stringBuilder = new StringBuilder();
        using var writer = new StreamWriter(dataFilePath);
            
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

    public void Start()
    {
        if (gnupProcess is not null || gnupSw is not null) Stop();
            
        gnupProcess = new Process();
        var startInfo = gnupProcess.StartInfo;

        startInfo.FileName = gnuplotPath;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardInput = true;

        gnupProcess.Start();

        gnupSw = gnupProcess.StandardInput;
        gnupSw.WriteLine($"plot '{dataFilePath}' w l");
    }

    public void Stop()
    {
        if (gnupProcess is null || gnupSw is null) return;
            
        gnupSw.Close();
        gnupSw.Dispose();
        gnupSw = null;
            
        gnupProcess.Close();
        gnupProcess.Dispose();
        gnupProcess = null;
    }
}