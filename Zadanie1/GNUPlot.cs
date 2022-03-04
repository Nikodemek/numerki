using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Zadanie1
{
    public class GNUPlot
    {
        private static readonly string dataPath = @"..\..\..\assets\data.dat";
        private static readonly string gnuplotPath = @"C:\Program Files\gnuplot\bin\gnuplot.exe";

        public static void FuncDataToFile(Func<double, double> expression, double min, double max, double step = 0.1d)
        {
            var stringBuilder = new StringBuilder();
            using var writer = new StreamWriter(dataPath);

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

        public static IDisposable Run()
        {
            using var gnuplotProcess = new Process();
            ProcessStartInfo startInfo = gnuplotProcess.StartInfo;

            startInfo.FileName = gnuplotPath;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;

            gnuplotProcess.Start();

            StreamWriter gnupStWr = gnuplotProcess.StandardInput;
            gnupStWr.WriteLine($"plot '{dataPath}' w l");
             
            return gnupStWr;
        }
    }
}