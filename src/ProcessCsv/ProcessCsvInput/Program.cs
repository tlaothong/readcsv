using System;
using System.IO;

namespace ProcessCsvInput
{
    class Program
    {
        private const string DefaultCsvFilePath = @"..\..\..\..\data\input.csv";
        private const string FallbackDefaultCsvFilePath = @"..\data\input.csv";

        static void Main(string[] args)
        {
            // Get CSV file to work with
            string csvPath = GetCsvPath(args);
        }

        private static string GetCsvPath(string[] args)
        {
            string csvPath = null;

            if (args?.Length > 1)
            {
                csvPath = args[0];
            }
            if (string.IsNullOrEmpty(csvPath))
            {
                csvPath = DefaultCsvFilePath;
            }
            if (!File.Exists(csvPath))
            {
                csvPath = FallbackDefaultCsvFilePath;
            }
            csvPath = Path.GetFullPath(csvPath);

            if (File.Exists(csvPath))
            {
                Console.WriteLine("Using CSV file: {0}", csvPath);
                return csvPath;
            }
            else
            {
                Console.WriteLine("CSV file not found. Program will terminate.");
                return null;
            }
        }
    }
}
