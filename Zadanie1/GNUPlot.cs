using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Zadanie1
{
    public class GNUPlot
    {
        public static void FuncDataToFile(Func<double, double> expression, double min, double max)
        {
            double x = min;
            double y;
            double step = 0.1d;
            
            string fullPath = "../../../assets/data.dat";
            StringBuilder stringBuilder = new StringBuilder();
            
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                while (x < max)
                {
                    stringBuilder.Clear();
                    
                    y = expression(x);
                    stringBuilder.Append(x);
                    stringBuilder.Append("\t");
                    stringBuilder.Append(y);

                    writer.WriteLine(stringBuilder.ToString().Replace(",", "."));
                    x += step;
                }
            }  
        }

        public static void Initialize()
        {
            string gnuplotPath = @"C:\Program Files\gnuplot\bin\gnuplot.exe";
            try
            {
                using (Process GNUPlot = new Process())
                {
                    GNUPlot.StartInfo.FileName = gnuplotPath;
                    GNUPlot.StartInfo.UseShellExecute = false;
                    GNUPlot.StartInfo.RedirectStandardInput = true;
                    GNUPlot.Start();

                    StreamWriter gnupStWr = GNUPlot.StandardInput;
                    gnupStWr.WriteLine("plot '../../../assets/data.dat' w l");
                    gnupStWr.Flush();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}