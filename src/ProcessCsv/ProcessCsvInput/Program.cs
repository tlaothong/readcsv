using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            var records = ReadCsvRecords(csvPath);

            Console.WriteLine("Reading {0} record(s) from {1}.", records.Count(), csvPath);
            Console.WriteLine();

            // TODO: Process your data
            foreach (var r in records)
            {
                Console.WriteLine("{0}, {1}", r.A, r.B);
            }
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

        private static IEnumerable<CsvInputModel> ReadCsvRecords(string csvPath)
        {
            using (var csvFile = File.OpenText(csvPath))
            {
                var csv = new CsvHelper.CsvReader(csvFile);
                csv.Configuration.BadDataFound = null;
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.HeaderValidated = (isValid, headerNames, headerNameIndex, csvContext) =>
                {
                    if ( ! isValid)
                    {
                        Console.WriteLine("There's a problem to a header of column '{0}'.", headerNames[headerNameIndex]);
                    }
                };

                return csv.GetRecords<CsvInputModel>().ToArray();
            }
        }
    }
}
