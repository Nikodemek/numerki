using System.Globalization;
using Zadanie3.Utils;

namespace Zadanie3.Dao;

public class FileManager : IFileReader<double[,]>
{
    private readonly string _fileName;
    private readonly string _filePath;

    public FileManager(string fileName)
    {
        if (String.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("FileName cannot be empty!", nameof(fileName));

        _fileName = fileName;
        _filePath = Path.Combine(Global.BaseDataDirPath, _fileName);

        Global.EnsureDirectoryIsValid();
        if (!File.Exists(_filePath)) throw new FileNotFoundException("File not found!", _filePath);

    }

    public double[,] Read()
    {
        var rawData = File.ReadAllLines(_filePath);
        return ClearData(rawData);
    }

    private static double[,] ClearData(string[] data)
    {
        int rows = data.Length;
        var clearedData = new List<double[]>();

        for (var i = 0; i < rows; i++)
        {
            var splitLine = data[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (splitLine.Length == 1) throw new InvalidDataException("Too few arguments in one line.");
            else if (splitLine.Length == 0) continue;

            var line = new double[2];
            for (var j = 0; j < 2; j++)
            {
                line[j] = Double.Parse(splitLine[j], NumberStyles.Float, NumberFormatInfo.InvariantInfo);
            }
            clearedData.Add(line);
        }

        return ArraysUtil.ConvertToTwoDimensional(clearedData);
    }
}