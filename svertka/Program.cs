using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace MatrixConvolution
{
    class Program
    {
        private static string dataDir = @"../../../../data/";
        private static string inputPath = dataDir + @"source.txt";
        private static int sampleSize = 20;

        static void Main(string[] args)
        {
            Directory.Delete(Path.Combine(dataDir, "out"), true);
            Directory.CreateDirectory(Path.Combine(dataDir, "out"));
            // Путь к файлу с исходной бинарной матрицей
            string outputPath = @"C:\Users\kat\RiderProjects\Signal\Signal\bin\Debug\net10.0\result.txt";

            // Читаем матрицу из файла
            int[,] matrix = Tools.ReadMatrixFromFile(inputPath);
            Tools.SaveMatrixAsBmp(matrix, Path.Combine(dataDir, "out/source.bmp"));
            for (int y = 0; y < matrix.GetLength(0) - sampleSize; y += sampleSize)
            {
                var sampleMatrix = new int[sampleSize, matrix.GetLength(1)];
                var sampleMatrixSize = sampleSize * matrix.GetLength(1);
                var sourceIndex = y * matrix.GetLength(1);
                Array.Copy(
                    matrix, sourceIndex,
                    sampleMatrix, 0,
                    sampleMatrixSize);
                Tools.SaveMatrixAsBmp(sampleMatrix, Path.Combine(dataDir, $"out/source.{y}.bmp"));
            }


            // // IConvolution alg = new ConvolutionChangeSize(inputPath);
            // IConvolution alg = new ConvolutionFixedSize(inputPath);
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
}