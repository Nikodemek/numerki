using Zadanie2.Utils;

namespace Zadanie2.Dao;

public class MatricesReader : IFileReader<double[][,]>
{
    private readonly string _fileName;
    private readonly string _filePath;

    public MatricesReader(string fileName)
    {
        if (String.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("FileName cannot be empty!", nameof(fileName));

        _fileName = fileName;
        _filePath = Path.Combine(Global.BaseDataDirPath, _fileName);

        if (!File.Exists(_filePath)) throw new FileNotFoundException("File not found!", _filePath);
    }

    //No trzyma się, na włosku ale trzyma. Na końcu pliku muszą być DWIE puste linie. It's a feature, not a bug.
    public double[][,] Read()
    {
        var data = File.ReadAllLines(_filePath);
        var dataLength = data.Length;

        var list = new List<double[,]>();
        var splitLines = new List<string[]>();

        int rows = 0;
        int columns = 0;
        for (int i = 0; i < dataLength; i++)
        {
            string[] splitLine = data[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            splitLines.Add(splitLine);
            int lineLength = splitLine.Length;
            if (lineLength == 0 || i == dataLength - 1)
            {
                var matrix = new double[rows, columns];

                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    for (int k = 0; k < matrix.GetLength(1); k++)
                    {
                        string valueString = splitLines[j][k];
                        matrix[j, k] = Parser.ToDouble(valueString);
                    }
                }

                list.Add(matrix);
                splitLines.Clear();
                rows = 0;
                continue;
            }
            columns = lineLength;
            rows++;
        }

        return list.ToArray();
    }
}