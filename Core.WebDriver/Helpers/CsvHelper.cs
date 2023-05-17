using Taf.UI.Core.Models.TestCase;
using System.Collections.Generic;
using System.IO;

namespace Taf.UI.Core.Helpers
{
    public class CsvHelper
    {
        public static string AppendToCsv<T>(T line, string filePath)
        {
            string err = string.Empty;

            string pathWithoutFileName = Path.GetDirectoryName(filePath);

            if (Directory.Exists(pathWithoutFileName))
            {
                using StreamWriter writer = File.AppendText(filePath);

                writer.WriteLine(line.ToString());
            }
            else
            {
                err = $"Failed to save file: {Path.GetFileName(filePath)} path does not exist: {pathWithoutFileName}";
            }

            return err;
        }

        public static List<CreateArticleTestCase> ReadCsv(string filePath)
        {
            List<CreateArticleTestCase> testCases = new List<CreateArticleTestCase>();

            if (File.Exists(filePath))
            {
                using StreamReader reader = new StreamReader(filePath);

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    var values = line.Split(';', System.StringSplitOptions.RemoveEmptyEntries);

                    CreateArticleTestCase testCase = new CreateArticleTestCase
                    {
                        ItemTitle = values.Length > 0 ? values[0] : string.Empty,
                        IsItemCreated = values.Length > 1 &&  values[1] == "True",
                        ItemOriginalId = values.Length > 2 ? values[2] : string.Empty,
                        ItemCurrentId = values.Length > 3 ? values[3] : string.Empty
                    };

                    testCases.Add(testCase);
                }
            }

            return testCases;
        }

        public static List<string> ReadCsvLines(string filePath)
        {
            List<string> lines = new List<string>();

            if (File.Exists(filePath))
            {
                using StreamReader reader = new StreamReader(filePath);

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    lines.Add(line);
                }
            }

            return lines;
        }
    }
}
