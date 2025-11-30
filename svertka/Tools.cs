using System.Drawing;
using System.Drawing.Imaging;

namespace MatrixConvolution;

public static class Tools
{
    public static void SaveMatrixAsBmp(int[,] matrix, string path)
    {
        int height = matrix.GetLength(0);
        int width = matrix.GetLength(1);

        using var bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            int v = matrix[y, x];
            Color color = v == 1 ? Color.Black : Color.White;
            bmp.SetPixel(x, y, color);
        }

        bmp.Save(path, ImageFormat.Bmp);
    }

    // Метод для записи матрицы в файл
    public static void WriteMatrixToFile(int[,] matrix, string path)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        string[] lines = new string[rows];

        for (int i = 0; i < rows; i++)
        {
            string[] rowValues = new string[cols];
            for (int j = 0; j < cols; j++)
            {
                rowValues[j] = matrix[i, j].ToString();
            }

            lines[i] = string.Join(" ", rowValues);
        }

        File.WriteAllLines(path, lines);
    }


    public static bool CompareMatrix(int[,]? a, int[,]? b)
    {
        if (a == null || b == null) return false;
        int ah = a.GetLength(0);
        int aw = a.GetLength(1);
        int bh = b.GetLength(0);
        int bw = b.GetLength(1);
        if (ah != bh || aw != bw) return false;

        for (int y = 0; y < ah; y++)
        {
            for (int x = 0; x < aw; x++)
            {
                if (a[y, x] != b[y, x]) return false;
            }
        }

        return true;
    }

    public static int[,] ReadMatrixFromFile(string path)
    {
        string[] lines = File.ReadAllLines(path);
        var firstLine = lines[0].ToCharArray();

        int rows = lines.Length;
        int cols = firstLine.Length;

        int[,] matrix = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            var values = lines[i].ToCharArray();
            if (values.Length == firstLine.Length)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = values[j] == '1' ? 1 : 0;
                }
            }
            else
            {
                Console.WriteLine($"Line out of range {i}");
            }
        }

        return matrix;
    }

    // Метод для вывода матрицы на экран
    public static void PrintMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] + " ");
            }

            Console.WriteLine();
        }
    }
}