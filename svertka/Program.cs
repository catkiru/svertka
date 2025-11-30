using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace MatrixConvolution
{
    class Program
    {
        static string inputPath = @"C:\Users\kat\RiderProjects\Signal\Signal\bin\Debug\net10.0\out.txt";

        static void Main(string[] args)
        {
            // Путь к файлу с исходной бинарной матрицей
            string outputPath = @"C:\Users\kat\RiderProjects\Signal\Signal\bin\Debug\net10.0\result.txt";

            // Читаем матрицу из файла
            int[,] matrix = Tools.ReadMatrixFromFile(inputPath);
            Tools.SaveMatrixAsBmp(matrix, inputPath + ".bmp");

            // IConvolution alg = new ConvolutionChangeSize(inputPath);
            IConvolution alg = new ConvolutionFixedSize(inputPath);

            // Применяем алгоритм свертки до стабилизации
            int[,] kernel = new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };
            bool isFound = alg.DoConvolution(matrix, kernel, out var resultMatrix);
            if (isFound)
            {
                Console.WriteLine("Сигнал найден");
                Tools.WriteMatrixToFile(resultMatrix, outputPath);
                Tools.SaveMatrixAsBmp(resultMatrix, outputPath + ".bmp");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Сигнал не найден");
            }
        }
    }
}