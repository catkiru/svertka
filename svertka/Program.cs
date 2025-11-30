using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace MatrixConvolution;

class Program
{
    public static List<(int[,] signal, int errors)> DetectSignal(int[,] matrix)
    {
        var result = new List<(int[,] signal, int errors)>();
        for (int x = 0; x < matrix.GetLength(1); x++)
        {
            int[,] test = new int[matrix.GetLength(0), matrix.GetLength(1)];
            bool detected = true;
            int errors = 0;
            // if (matrix[0, x] == 0)
            // {
            //    continue;
            // }

            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                int sum = (x <= 0 ? 0 : matrix[y, x - 1])
                          + matrix[y, x + 0]
                          + (x >= matrix.GetLength(1) - 1 ? 0 : matrix[y, x + 1]);
                if (sum > 0)
                {
                    if (matrix[y, x - 0] == 0)
                    {
                        errors++;
                    }

                    if (x - 1 >= 0)
                    {
                        test[y, x - 1] = matrix[y, x - 1];
                    }

                    test[y, x + 0] = matrix[y, x + 0];
                    if (x < matrix.GetLength(1) - 1)
                    {
                        test[y, x + 1] = matrix[y, x + 1];
                    }
                }
                else
                {
                    detected = false;
                    break;
                }
            }

            if (detected)
            {
                result.Add((test, errors));
            }
        }

        return result;
    }

    private static string dataDir = @"../../../../data/";
    private static string inputPath = dataDir + @"source.txt";
    private static int sampleSize = 40;

    static void Main(string[] args)
    {
        Directory.Delete(Path.Combine(dataDir, "out"), true);
        Directory.CreateDirectory(Path.Combine(dataDir, "out"));
        // Путь к файлу с исходной бинарной матрицей
        string outputPath = @"C:\Users\kat\RiderProjects\Signal\Signal\bin\Debug\net10.0\result.txt";

        // Читаем матрицу из файла
        int[,] matrix = Tools.ReadMatrixFromFile(inputPath);
        Tools.SaveMatrixAsBmp(matrix, Path.Combine(dataDir, "out/source.bmp"));
        IConvolution alg = new ConvolutionFixedSize(inputPath);
        int[,] kernel = new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };
        bool isFound = alg.DoConvolution(matrix, kernel, out var convolutedMatric);
        Tools.SaveMatrixAsBmp(convolutedMatric, Path.Combine(dataDir, "out/convoluted.bmp"));

        for (int y = 0; y < matrix.GetLength(0) - sampleSize; y += sampleSize)
        {
            var sampleMatrix = new int[sampleSize, convolutedMatric.GetLength(1)];
            var sampleMatrixSize = sampleSize * convolutedMatric.GetLength(1);
            var sourceIndex = y * convolutedMatric.GetLength(1);
            Array.Copy(
                convolutedMatric, sourceIndex,
                sampleMatrix, 0,
                sampleMatrixSize);
            Tools.SaveMatrixAsBmp(sampleMatrix, Path.Combine(dataDir, $"out/signals.{y}.bmp"));
            var signals = DetectSignal(sampleMatrix);
            Tools.SaveMatrixAsBmp(sampleMatrix, signals, Path.Combine(dataDir, $"out/signals.{y}.one.bmp"));
            Tools.SaveMatrixAsBmp2(sampleMatrix, signals, Path.Combine(dataDir, $"out/signals.{y}.two.bmp"));
            File.AppendAllLines(Path.Combine(dataDir, "out/result.txt"),
            [
                $"sample: {y} Signals: {signals.Count}, errors: {string.Join("; ", signals.Select((i, x) => i.errors))}"
            ]);
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
}