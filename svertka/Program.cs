namespace MatrixConvolution;

class Program
{
    private static string dataDir = @"../../../../data/";
    private static string inputPath = dataDir + @"source.txt";

    static void Main(string[] args)
    {
        Directory.CreateDirectory(Path.Combine(dataDir, "out"));
        Directory.Delete(Path.Combine(dataDir, "out"), true);
        Directory.CreateDirectory(Path.Combine(dataDir, "out"));


        int[] sampleSizes = [40, 30, 20];

        foreach (var sampleSize in sampleSizes)
        {
            ProcessSample(sampleSize);
        }


        // // IConvolution alg = new ConvolutionChangeSize(inputPath);
        //
        // // Применяем алгоритм свертки до стабилизации
        // int[,] kernel = new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };
        // bool isFound = alg.DoConvolution(matrix, kernel, out var resultMatrix);
        // if (isFound)
        // {
        //     Console.WriteLine("Сигнал найден");
        //     Tools.WriteMatrixToFile(resultMatrix, outputPath);
        //     Tools.SaveMatrixAsBmp(resultMatrix, outputPath + ".bmp");
        // }
        // else
        // {
        //     Console.ForegroundColor = ConsoleColor.Red;
        //     Console.WriteLine("Сигнал не найден");
        // }
    }

    private static void ProcessSample(int sampleSize)
    {
        var outputPath = Path.Combine(dataDir, "out", sampleSize.ToString());
        // Читаем матрицу из файла
        int[,] matrix = Tools.ReadMatrixFromFile(inputPath);
        Directory.CreateDirectory(outputPath);
        Tools.SaveMatrixAsBmp(matrix, Path.Combine(outputPath, "source.bmp"));
        IConvolution alg = new ConvolutionFixedSize(inputPath);
        int[,] kernel = new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };
        bool isFound = alg.DoConvolution(matrix, kernel, out var convolutedMatric);
        Tools.SaveMatrixAsBmp(convolutedMatric, Path.Combine(outputPath, "convoluted.bmp"));

        for (int y = 0; y < matrix.GetLength(0) - sampleSize; y += sampleSize)
        {
            var sampleMatrix = new int[sampleSize, convolutedMatric.GetLength(1)];
            var sampleMatrixSize = sampleSize * convolutedMatric.GetLength(1);
            var sourceIndex = y * convolutedMatric.GetLength(1);
            Array.Copy(
                convolutedMatric, sourceIndex,
                sampleMatrix, 0,
                sampleMatrixSize);
            Tools.SaveMatrixAsBmp(sampleMatrix, Path.Combine(outputPath, $"signals.{y}.bmp"));
            var signals = Tools.DetectSignal(sampleMatrix);
            Tools.SaveMatrixAsBmp(sampleMatrix, signals, Path.Combine(outputPath, $"signals.{y}.one.bmp"));
            Tools.SaveMatrixAsBmp2(sampleMatrix, signals, Path.Combine(outputPath, $"signals.{y}.two.bmp"));
            File.AppendAllLines(Path.Combine(outputPath, "result.txt"),
            [
                $"sample: {y} Signals: {signals.Count}, errors: {string.Join("; ", signals.Select((i, x) => i.errors))}"
            ]);
        }
    }
}