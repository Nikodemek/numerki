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

    private double[,] ClearData(string[] data)
    {
        int rowLength = data.Length;

        List<double[]> clearedData = new List<double[]>();

        for (var i = 0; i < rowLength; i++)
        {
            string[] splitLine = data[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (splitLine.Length == 1)
                throw new InvalidDataException("Too few arguments in one line.");
            if (splitLine.Length == 0)
                continue;

            clearedData.Add(new double[2]);
            
            for (var j = 0; j < 2; j++)
                clearedData[i][j] = Convert.ToDouble(splitLine[j]);
        }

        return ArrayConverter.ConvertToTwoDimensional(clearedData);
    }
}