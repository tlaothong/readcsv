using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProcessCsvInput
{
    class Program
    {
        private const string DefaultInputCsvFileName = "input.csv";
        private const string DefaultCsvFolder = @"..\..\..\..\data";
        private const string FallbackDefaultCsvFolder = @"..\data";

        static void Main(string[] args)
        {
            // Get CSV filename from CLI argument or use default filename (input.csv).
            var csvFilename = (args?.Length > 0) ? args[0] : DefaultInputCsvFileName;
            var records = ReadCsvRecords<CsvInputModel>(csvFilename);

            Console.WriteLine("Reading {0} record(s) from {1}.", records.Count(), csvFilename);
            Console.WriteLine();

            // TODO: Process your data
            foreach (var r in records)
            {
                Console.WriteLine("{0}, {1}", r.A, r.B);
            }
        }

        private static string SearchMatchingCsvPath(string csvFilename)
        {
            string csvPath = csvFilename;

            if (!File.Exists(csvPath) && !Path.IsPathFullyQualified(csvFilename))
            {
                csvPath = Path.Combine(DefaultCsvFolder, csvFilename);
            }
            if (!File.Exists(csvPath) && !Path.IsPathFullyQualified(csvFilename))
            {
                csvPath = Path.Combine(FallbackDefaultCsvFolder, csvFilename);
            }
            csvPath = Path.GetFullPath(csvPath);

            if (File.Exists(csvPath))
            {
                Console.WriteLine("Found CSV file: {0}", csvPath);
                return csvPath;
            }
            else
            {
                Console.WriteLine("CSV file not found. Program will terminate.");
                return null;
            }
        }

        private static IEnumerable<T> ReadCsvRecords<T>(string csvFilename)
        {
            var csvPath = SearchMatchingCsvPath(csvFilename);

            if (string.IsNullOrEmpty(csvPath))
            {
                throw new ApplicationException("Program CAN'T be continue without the correct file.");
            }

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

                return csv.GetRecords<T>().ToArray();
            }
        }
    }
}
